import { Observable, Subject } from "rxjs-compat";

import "rxjs/add/observable/dom/ajax";
import "rxjs/add/operator/map";
import "rxjs/add/operator/share";
import "rxjs/add/operator/startWith";
import "rxjs/add/operator/switchMap";
import { debug } from "../shared/operators/debug";

import { View } from "../shared/models";

const requestDeleteViewSubject = new Subject<View | null>();
export const requestDeleteView = (view: View | null) => requestDeleteViewSubject.next(view);
export const deleteViewRequests: Observable<View | null> = requestDeleteViewSubject.startWith(null);

const confirmDeleteViewSubject = new Subject<View>();
export const confirmDeleteView = (view: View) => confirmDeleteViewSubject.next(view);
export const deletedViews = confirmDeleteViewSubject
  .switchMap(view => Observable.ajax
    .delete(`api/views/${view.id}`)
    .map(xhr => view))
  .pipe(debug("Deleted view"))
  .share();