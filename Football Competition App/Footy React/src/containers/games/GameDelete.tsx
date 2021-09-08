import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useContext, useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { EDelete } from "../../types/EDelete";
import { Redirect } from "react-router";
import { AppContext } from "../../context/AppContext";
import { IGame } from "../../dto/IGame";
import { ICompetition } from "../../dto/ICompetition";


const GameDelete = () => {

    const { id } = useParams() as IRouteId;

    const appState = useContext(AppContext);
    const [game, setGame] = useState({} as IGame);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });
    const [belongsToUser] = useState({ belongs: false });

    const loadData = async () => {
        let result = await BaseService.get<IGame>('/Games/' + id, appState.token!);
        let belongsResult = await BaseService.get<ICompetition>('/Competitions/userCompetitions?id=' + id, appState.token!);

        if (result.ok && result.data && belongsResult) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            belongsToUser.belongs = true;
            setGame(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<IGame>('/Games/' + id, appState.token!);
        if (response.ok) {
            setDelete({deleteStatus: EDelete.Deleted});
        }
    }

    if (appState.token === null) {
        return <><Redirect to={"/"}/></>
    }

    if (game && game.home && game.away && game.competition) {
        return (
            <>
            { !belongsToUser ? <Redirect to="/" /> : null}
            { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/games/organiser" /> : null}
                <form onSubmit={(e) => deleteClicked(e.nativeEvent)}>
                    <h5 className="text-center">{game.home.name} vs {game.away.name}</h5>
                    <h4 className="text-center">Are you sure you want to delete this?</h4>

                    <div>

                        <hr />
                        <dl className="row">
                            <dt className="col-sm-4">
                                Home team
                            </dt>
                            <dd className="col-sm-8">
                                {game.home.name}
                            </dd>
                            <dt className="col-sm-4">
                                Away team
                            </dt>
                            <dd className="col-sm-8">
                                {game.away.name}
                            </dd>
                            <dt className="col-sm-4">
                                Competition
                            </dt>
                            <dd className="col-sm-8">
                                {game.competition.name}
                            </dd>
                            <dt className="col-sm-4">
                                Home score
                            </dt>
                            <dd className="col-sm-8">
                                {game.homeScore}
                            </dd>
                            <dt className="col-sm-4">
                                Away score
                            </dt>
                            <dd className="col-sm-8">
                                {game.awayScore}
                            </dd>
                            <dt className="col-sm-4">
                                Comment
                            </dt>
                            <dd className="col-sm-8">
                                {game.comment}
                            </dd>
                        </dl>
                        <div className="form-group">
                            <button onClick={(e) => deleteClicked(e.nativeEvent)} type="submit" className="btn btn-danger">Delete</button>
                        </div>
                    </div>
                </form>
            </>
        );
    }
    return (
        <>
            <Loader {...pageStatus} />
        </>
    );
}

export default GameDelete;