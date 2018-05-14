import { map, share, switchMap } from 'rxjs/operators';
import { Observable, Subject } from "rxjs";

import { debug } from "../shared/operators/debug";

import { View } from "../shared/models";
import { IView } from "../shared/contracts";
import { ajax } from "rxjs/ajax";

const savedViewsSubject = new Subject<View>();
export const saveView = (view: View) => savedViewsSubject.next(view);
export const savedViews: Observable<View> = savedViewsSubject.pipe(
  switchMap(savedView => ajax
    .post("api/views", savedView, {"Content-Type": "application/json"}).pipe(
      map(xhr => xhr.response as IView),
      map(View.fromContract),)
  ))
  .pipe(debug("Saved view"))
  .pipe(share());
