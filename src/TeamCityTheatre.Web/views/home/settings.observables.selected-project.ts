
import {map, startWith, share, switchMap} from 'rxjs/operators';
import {Observable, Subject} from "rxjs-compat";

import { debug } from "../shared/operators/debug";
import "rxjs/add/observable/dom/ajax";
import "rxjs/add/operator/map";
import "rxjs/add/operator/share";
import "rxjs/add/operator/startWith";
import "rxjs/add/operator/switchMap";

import {IDetailedProject} from "../shared/contracts";
import {BuildConfiguration, Project} from "../shared/models";

const selectedProjectsSubject = new Subject<Project>();
export const selectProject = (project: Project) => selectedProjectsSubject.next(project);

export const selectedProjects: Observable<Project | null> = selectedProjectsSubject.pipe(
  switchMap(project => Observable.ajax.getJSON<IDetailedProject>(`api/projects/${project.id}`).pipe(
    map(detailedProject => project.withBuildConfigurations(detailedProject.buildConfigurations.map(BuildConfiguration.fromContract))),
    startWith(null),)))
  .pipe(debug("Selected project")).pipe(
  startWith(null),
  share(),);
