import { useContext, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Loader from "../../components/Loader";
import { AppContext } from "../../context/AppContext";
import { IGame } from "../../dto/IGame";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { IRouteId } from "../../types/IRouteId";

const RowDisplay = (props: { game: IGame, allowed: string }) => {
    var dateformat = require("dateformat");
    const appState = useContext(AppContext);

    return (
        <tr>
            <td>
                {props.game.competition!.name}
            </td>
            <td>
                {props.game.kickOffTime ? dateformat(props.game.kickOffTime, "dd/mm/yyyy HH:MM") : ""}
            </td>
            <td>
                {props.game.home!.name} vs {props.game.away!.name}
            </td>
            <td>
                {props.game.homeScore} - {props.game.awayScore}
            </td>
            <td>
                {props.game.comment}
            </td>
            <td>
                {appState.role === "Admin" || (appState.token !== null && props.allowed === "organiser") ?
                    <>
                        <Link className="text-dark" to={'/games/edit/' + props.game.id}>Edit</Link>
                        |
                        <Link className="text-dark" to={'/games/delete/' + props.game.id}>Delete</Link>
                    </> : null
                }
            </td>
        </tr>
    );
}

const GameIndex = () => {
    
    const [games, setGames] = useState([] as IGame[]);
    const [title, setTitle] = useState({ name: "All games" });
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [currentId, setCurrentId] = useState({ current: "" });
    const appState = useContext(AppContext);
    const { id } = useParams() as IRouteId;

    const loadData = async () => {
        let gameUri = "/Games";

        if (id === "team" && appState.token !== null) {
            gameUri += "/teamManager"
            setTitle({ name: "My teams at games" });
            setCurrentId({ current: id });
        } else if (id === "organiser" && appState.token !== null) {
            gameUri +="/organiser";
            setTitle({ name: "Games played at my competitions" });
            setCurrentId({ current: id });
        }

        let result = await BaseService.getAll<IGame>(gameUri, appState.token);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setGames(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    if (id !== currentId.current && id !== "" && id !== undefined) {
        loadData();
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps


    return (
        <>
            <h4 className="text-center">{title.name}</h4>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                            Competition
                        </th>
                        <th>
                            Kickoff time
                        </th>
                        <th>
                            Game
                        </th>
                        <th>
                            Score
                        </th>
                        <th>
                            Comment
                        </th>
                        {appState.role === "Admin" ?
                            <> <th></th> </>
                            :
                            null
                        }
                    </tr>
                </thead>
                <tbody>
                    {games.map(game =>
                        <RowDisplay game={game} allowed={id} key={game.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

export default GameIndex;