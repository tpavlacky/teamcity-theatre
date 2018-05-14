
import {defer as observableDefer} from 'rxjs';

import {merge, scan, startWith, map, switchMap} from 'rxjs/operators';
import { Observable, Subject } from "rxjs-compat";

import { debug } from "../shared/operators/debug";

import "rxjs/add/observable/defer";
import "rxjs/add/observable/dom/ajax";

import "rxjs/add/operator/map";
import "rxjs/add/operator/merge";
import "rxjs/add/operator/scan";
import "rxjs/add/operator/startWith";
import "rxjs/add/operator/switchMap";

import { IBasicProject } from "../shared/contracts";
import { Project } from "../shared/models";
import { selectedProjects } from "./settings.observables.selected-project";

const toProjects = (basicProjects: IBasicProject[]) => {
  const projects = basicProjects.map(p => new Project(p));

  const findChildren = (id: string) => projects.filter(p => p.parentProjectId === id);

  for (let project of projects) {
    project.setChildren(findChildren(project.id));
  }

  return projects;
};

const initialRootProjects: Observable<Project> = observableDefer(() => Observable.ajax.getJSON<IBasicProject[]>("api/projects")).pipe(
  map(toProjects),
  map(projects => projects.filter(p => p.parentProjectId === null)[0]),
  map(rootProject => rootProject.expand()),)
  .pipe(debug("Initial root project"));

const manualProjectUpdates = new Subject<Project>();
export const updateProject = (project: Project) => manualProjectUpdates.next(project);

const projectUpdates: Observable<Project | null> = manualProjectUpdates.pipe(merge(selectedProjects))
  .pipe(debug("Project update"));

export const rootProjects: Observable<Project> = initialRootProjects.pipe(switchMap((initialRootProject: Project) =>
  projectUpdates.pipe(
    scan<Project | null, Project>((previousRootProject: Project, projectUpdate: Project | null) => previousRootProject.update(projectUpdate), initialRootProject),
    startWith(initialRootProject),)))
  .pipe(debug("Projects"));
