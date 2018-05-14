
import {scan, merge, startWith, switchMap, map} from 'rxjs/operators';
import { Observable, Subject } from "rxjs-compat";

import { debug } from "../shared/operators/debug";

import "rxjs/add/observable/defer";
import "rxjs/add/observable/dom/ajax";
import "rxjs/add/operator/map";
import "rxjs/add/operator/merge";
import "rxjs/add/operator/scan";
import "rxjs/add/operator/startWith";
import "rxjs/add/operator/switchMap";

import { IView } from "../shared/contracts";
import { View } from "../shared/models";
import { savedViews } from "./settings.observables.save-view";
import { mergeById } from "../shared/arrays/mergeById";
import { deletedViews } from "./settings.observables.delete-view";

const updatedViewsSubject = new Subject<View>();
export const updateView = (view: View) => {
  updatedViewsSubject.next(view);
  return view
};
export const updatedViews: Observable<View> = updatedViewsSubject.pipe(merge(savedViews)).pipe(debug("Update view"));

const initialViews: Observable<View[]> = deletedViews.pipe( // every time a view is deleted, we fetch the list of views again
  startWith({}),
  switchMap(() => Observable.ajax.getJSON<IView[]>("api/views")),
  map(vs => vs.map(View.fromContract)),
  startWith([]),)
  .pipe(debug("Initial views"));

export const views: Observable<View[]> = initialViews.pipe(switchMap(initialVs =>
  updatedViews.pipe(
    scan((previousViews, updatedView) => mergeById(updatedView, previousViews), initialVs),
    startWith(initialVs),)))

  .pipe(debug("Views"));