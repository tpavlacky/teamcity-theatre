
import {combineLatest as observableCombineLatest, of as observableOf, empty as observableEmpty, defer as observableDefer} from 'rxjs';

import {startWith, switchMap, delay, mergeMap, repeat, merge, catchError} from 'rxjs/operators';
import {Observable, Subject} from "rxjs-compat";

import "rxjs/add/observable/dom/ajax";
import "rxjs/add/observable/of";
import "rxjs/add/observable/defer";
import "rxjs/add/observable/combineLatest";
import "rxjs/add/observable/empty";

import "rxjs/add/operator/catch";
import "rxjs/add/operator/delay";
import "rxjs/add/operator/merge";
import "rxjs/add/operator/mergeMap";
import "rxjs/add/operator/repeat";
import "rxjs/add/operator/switchMap";
import "rxjs/add/operator/startWith";

import {IView, IViewData} from "../shared/contracts";

// fetching the initial set of views

export const allViews: Observable<IView[] | null> = observableDefer(() => Observable.ajax.getJSON<IView[]>("api/views"));

// selecting a view

const selectedViewsSubject = new Subject<IView>();
export const selectView = (view: IView) => selectedViewsSubject.next(view);
export const selectedViews: Observable<IView | null> = selectedViewsSubject;

// fetching the data of a view
export const selectedViewData : Observable<IViewData | null> = selectedViews.pipe(
  switchMap(
  (selectedView: IView | null) => selectedView == null
    ? observableEmpty()
    : observableOf(null).pipe(
                delay(3000),
                mergeMap(() => Observable.ajax.getJSON<IViewData>(`api/viewdata/${selectedView.id}`).pipe(
                  catchError(() => observableEmpty()))),
                repeat(),
                merge(Observable.ajax.getJSON<IViewData>(`api/viewdata/${selectedView.id}`).pipe(
                  catchError(() => observableEmpty()))),)
));

// combining all of the above in a single state

export interface IDashboardState {
  views: IView[] | null;
  selectedView: IView | null;
  selectedViewData: IViewData | null;
}

export const state : Observable<IDashboardState> = observableCombineLatest(
  allViews.pipe(startWith(null)),
  selectedViews.pipe(startWith(null)),
  selectedViewData.pipe(startWith(null)),
  (views: IView[] | null, selectedView: IView | null, viewData: IViewData | null) => {
    const s: IDashboardState = {
      views: views,
      selectedView: selectedView,
      selectedViewData: viewData
    };
    return s;
  });