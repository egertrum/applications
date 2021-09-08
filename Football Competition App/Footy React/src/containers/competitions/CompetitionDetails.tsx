import { Link, useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { ICompetition } from "../../dto/ICompetition";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { ICompetitionTeam } from "../../dto/ICompetitionTeam";
import pin from '../../wwwroot/images/pin.png';
import { IGame } from "../../dto/IGame";
var dateformat = require("dateformat");


const RowDisplay = (props: { competitionTeam: ICompetitionTeam }) => {

    return (
        <tr>
            <td>
            <img className="very-small-icon" src={pin} alt="" />
                {props.competitionTeam.team!.country!.name}
            </td>
            <td>
                {props.competitionTeam.team!.name}
            </td>
            <td>
                {props.competitionTeam.team!.playersAmount}
            </td>
            <td>
                {dateformat(props.competitionTeam.since, "dd/mm/yyyy")}
            </td>
            <td>
                <Link className="text-dark" to={'/teams/' + props.competitionTeam.teamId}>More Info</Link>
            </td>
        </tr>
    );
}

const GameRowDisplay = (props: { game: IGame }) => {

    return (
        <tr>
            <td>
            <img className="very-small-icon" src={pin} alt="" />
                {props.game.competition!.name}
            </td>
            <td>
                {dateformat(props.game.kickOffTime, "dd/mm/yyyy")}
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
        </tr>
    );
}

const CompetitionDetails = () => {

    const { id } = useParams() as IRouteId;
    const [competition, setCompetition] = useState({} as ICompetition);
    const [competitionTeams, setCompetitionTeams] = useState([] as ICompetitionTeam[]);
    const [games, setGames] = useState([] as IGame[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });

    useEffect(() => {
        const loadData = async () => {
            let compsResult = await BaseService.get<ICompetition>('/Competitions/' + id);
            let teamsResult = await BaseService.getAll<ICompetitionTeam>('/CompetitionTeams/competition?competitionId=' + id);
            let gamesResult = await BaseService.getAll<IGame>('/Games/competition?id=' + id); 

            if (compsResult.ok && compsResult.data && teamsResult.ok && teamsResult.data && gamesResult.ok && gamesResult.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setCompetition(compsResult.data);
                setGames(gamesResult.data);
                setCompetitionTeams(teamsResult.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: 500 });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (competition && competition.country && competitionTeams && games) {
        return (
            <>
                <h4 className="text-center">{competition.name}</h4>

                <div>

                    <hr />
                    <dl className="row">
                        <dt className="col-sm-2">
                            Country
                        </dt>
                        <dd className="col-sm-10">
                            {competition.country.name}
                        </dd>
                        <dt className="col-sm-2">
                            Competition name
                        </dt>
                        <dd className="col-sm-10">
                            {competition.name}
                        </dd>
                        <dt className="col-sm-2">
                            Organiser
                        </dt>
                        <dd className="col-sm-10">
                            {competition.organiser}
                        </dd>
                        <dt className="col-sm-2">
                            Start date
                        </dt>
                        <dd className="col-sm-10">
                            {dateformat(competition.startDate, "dd/mm/yyyy")}
                        </dd>
                        <dt className="col-sm-2">
                            End date
                        </dt>
                        <dd className="col-sm-10">
                            {dateformat(competition.endDate, "dd/mm/yyyy")}
                        </dd>
                        <dt className="col-sm-2">
                            Comment
                        </dt>
                        <dd className="col-sm-10">
                            {competition.comment}
                        </dd>
                    </dl>
                </div>
                <hr />
                <h4 className="text-center">Teams attending this competition</h4>
                <div className="row-cols-1">
                    <div className="text-center">
                        <table className="table table-hover">
                            <thead>
                                <tr>
                                    <th>
                                    </th>
                                    <th>
                                        Team
                                    </th>
                                    <th>
                                        Amount of players
                                    </th>
                                    <th>
                                        Date registered
                                    </th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {competitionTeams.map(competitionTeam =>
                                    <RowDisplay competitionTeam={competitionTeam} key={competitionTeam.id} />)
                                }
                            </tbody>
                        </table>
                    </div>
                </div>
                <hr />
                <h4 className="text-center">Games</h4>
                <div className="row-cols-1">
                    <div className="text-center">
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
                                </tr>
                            </thead>
                            <tbody>
                                {games.map(game =>
                                    <GameRowDisplay game={game} key={game.id} />)
                                }
                            </tbody>
                        </table>
                    </div>
                </div>
            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}


export default CompetitionDetails;