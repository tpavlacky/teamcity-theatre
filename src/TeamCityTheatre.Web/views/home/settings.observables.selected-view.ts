import { scan, startWith, switchMap } from 'rxjs/operators';
import { Observable, Subject } from "rxjs";

import { debug } from "../shared/operators/debug";

import { View } from "../shared/models";
import { updatedViews } from "./settings.observables.views";

const selectedViewsSubject = new Subject<View>();
export const selectView = (view: View) => selectedViewsSubject.next(view);

export const selectedViews: Observable<View | null> = selectedViewsSubject.pipe(
  startWith<View | null>(null),
  switchMap((selectedView: View | null) => updatedViews.pipe(
    scan((previouslySelectedView: View | null, updatedView: View) =>
      previouslySelectedView !== null && previouslySelectedView.id === updatedView.id
        ? updatedView
        : previouslySelectedView, selectedView),
    startWith(selectedView),)
  ),)
  .pipe(debug("Selected view"));
