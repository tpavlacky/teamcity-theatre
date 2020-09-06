﻿import { addSeconds, formatDistanceStrict, formatDistanceToNow, parseISO } from "date-fns";
import { createElement, FunctionComponent, MouseEvent } from "react";
import { BuildStatus, IDetailedBuild, ITileData, IView, IViewData } from "../Shared/contracts";
import { IDashboardState } from "./dashboard.observables";

/**
 * Root dispatching component
 */
export const Dashboard: FunctionComponent<IDashboardState> =
    props => {
        if (props.views === null)
            return <div>
                <i className="fa fa-spin fa-cog"/> Loading views
            </div>;

        if (props.selectedView === null)
            return <Views views={ props.views }/>;

        if (props.selectedViewData === null)
            return <div>
                <i className="fa fa-spin fa-cog"/> Loading view data
            </div>;

        return <View view={ props.selectedView } data={ props.selectedViewData }/>;
    };

/**
 * List of views to choose from
 */
const Views = (props: { views: IView[] }) => (
    <div id="views">
        { props.views.map(view => (
            <a className="btn btn-primary view" id={ view.id } key={ view.id } href={ `dashboard/${ view.name.toLowerCase() }` }>
                { view.name } <span className="badge">{ view.tiles.length } tiles</span>
            </a>)) }
    </div>
);

const tryRequestFullScreen = (event: MouseEvent<HTMLButtonElement>) => {
  const button = event.currentTarget as HTMLButtonElement;
  const view = (button.parentElement as HTMLDivElement).getElementsByClassName("view").item(0);
  if (view != null && view.requestFullscreen) {
    view.requestFullscreen();
  };
}
/**
 * Details of a single view
 */
const View = (props: { view: IView, data: IViewData }) => (
  <div>
    <button role="button" className="btn btn-primary btn-xs" onClick={tryRequestFullScreen}>
      <i className="fa fa-expand" /> Full screen
    </button>

    <div className="view" id={props.view.id}>
      <div id="tiles">
        <div className="tiles-wrapper">
          {props.data.tiles.map(tile => <Tile key={tile.id} view={props.view} data={tile} />)}
        </div>
      </div>
    </div>
  </div>
);

/**
 * A single tile of a view
 */
const Tile = (props: { view: IView, data: ITileData }) => {
    const buildStatus = BuildStatus[props.data.combinedBuildStatus].toLowerCase();
    const height = `height-${ props.view.defaultNumberOfBranchesPerTile }`;
    const numberOfColumns = props.view.numberOfColumns || 6;
    const width = `width-${ numberOfColumns }`;
    return (
        <div id={ props.data.id } className={ `tile ${ buildStatus } ${ height } ${ width }` }>
            <h4 className="tile-title">{ props.data.label }</h4>
            { props.view.defaultNumberOfBranchesPerTile > 0
                ?
                <div className="tile-builds">
                    { props.data.builds.map(build => <Build key={ build.id } build={ build }/>) }
                </div>
                : null
            }
        </div>
    );
};

/**
 * A single build in a tile
 */
const Build = (props: { build: IDetailedBuild }) => {
    const { build } = props;
    const isFinished = build.state === "finished";
    const isRunning = build.state === "running";
    const isSuccess = build.status === BuildStatus.Success;

    const buildStatus = BuildStatus[build.status].toLowerCase();
    const percentageCompleted = isFinished ? 100 : build.percentageComplete;
    const progressBarTheme = isSuccess ? "progress-bar-success" : "progress-bar-danger";
    const progressBarAnimation = isRunning ? "progress-bar-striped active" : "";

    return (
        <div id={ build.id } className={ `tile-build ${ buildStatus }` }>
            <div className="progress">
                <a href={ build.webUrl } target="_blank">
                    <div className={ `progress-bar ${ progressBarTheme } ${ progressBarAnimation }` }
                         style={ { width: `${ percentageCompleted }%` } }>
                        { <Branch build={ build }/> }
                        { isFinished ? <FinishDate build={ build }/> : null }
                        { isRunning ? <TimeRemaining build={ build }/> : null }
                    </div>
                </a>
            </div>
        </div>
    );
};

const Branch = (props: { build: IDetailedBuild }) => {
    const isDefaultBranch = props.build.isDefaultBranch;
    const branchDisplayName = props.build.branchName || props.build.number;
    return isDefaultBranch
        ? <span className="branch"><i className="fa fa-star"/> { branchDisplayName }</span>
        : <span className="branch">{ branchDisplayName }</span>;
};

const FinishDate = (props: { build: IDetailedBuild }) => {
    const { build } = props;
    const isSuccess = build.status === BuildStatus.Success;
    const theme = isSuccess ? "success" : "danger";

    const startDate = parseISO(build.startDate);
    const finishDate = parseISO(build.finishDate);
    const differenceWithNow = formatDistanceToNow(finishDate, { includeSeconds: true, addSuffix: true });
    const differenceWithStartDate = formatDistanceStrict(finishDate, startDate);
    return (
        <span className="execution-timestamp">
      <span className={ `build-number label label-${ theme }` }><i className="fa fa-tag"/> { build.number }</span>
      <span className="build-execution-time"> <i className="fa fa-clock-o"/> { differenceWithStartDate }</span>
      <span className="build-age"> ({ `${ differenceWithNow }` })</span>
    </span>
    );
};

const TimeRemaining = (props: { build: IDetailedBuild }) => {
    const { build } = props;
    const isSuccess = build.status === BuildStatus.Success;
    const theme = isSuccess ? "success" : "danger";

    const estimatedFinishDate = addSeconds(parseISO(props.build.startDate), props.build.estimatedTotalSeconds);
    const differenceWithNow = formatDistanceToNow(estimatedFinishDate, { includeSeconds: true, addSuffix: true });
    return (
        <span className="remaining">
      <span className={ `build-number label label-${ theme }` }>{ build.number }</span>
            { ` will finish ${ differenceWithNow }` }
    </span>
    );
};