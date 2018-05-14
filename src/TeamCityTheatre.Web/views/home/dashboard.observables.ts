import {
  combineLatest as observableCombineLatest,
  defer as observableDefer,
  empty as observableEmpty,
  Observable,
  of as observableOf,
  Subject
} from "rxjs";

import { catchError, delay, merge, mergeMap, repeat, startWith, switchMap } from 'rxjs/operators';
import { ajax } from "rxjs/ajax";
import { IView, IViewData } from "../shared/contracts";

// fetching the initial set of views

export const allViews: Observable<IView[] | null> = observableDefer(() => ajax.getJSON<IView[]>("api/views"));

// selecting a view

const selectedViewsSubject = new Subject<IView>();
export const selectView = (view: IView) => selectedViewsSubject.next(view);
export const selectedViews: Observable<IView | null> = selectedViewsSubject;

// fetching the data of a view
export const selectedViewData: Observable<IViewData | null> = selectedViews.pipe(
  switchMap(
    (selectedView: IView | null) => selectedView == null
      ? observableEmpty()
      : observableOf(null).pipe(
        delay(3000),
        mergeMap(() => ajax.getJSON<IViewData>(`api/viewdata/${selectedView.id}`).pipe(
          catchError(() => observableEmpty()))),
        repeat(),
        merge(ajax.getJSON<IViewData>(`api/viewdata/${selectedView.id}`).pipe(
          catchError(() => observableEmpty()))),)
  ));

// combining all of the above in a single state

export interface IDashboardState {
  views: IView[] | null;
  selectedView: IView | null;
  selectedViewData: IViewData | null;
}

export const state: Observable<IDashboardState> = observableCombineLatest(
  allViews.pipe(startWith<IView[] | null>(null)),
  selectedViews.pipe(startWith<IView | null>(null)),
  selectedViewData.pipe(startWith<IViewData | null>(null)),
  (views: IView[] | null, selectedView: IView | null, viewData: IViewData | null) => {
    const s: IDashboardState = {
      views: views,
      selectedView: selectedView,
      selectedViewData: viewData
    };
    return s;
  });