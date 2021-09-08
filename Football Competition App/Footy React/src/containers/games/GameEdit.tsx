import React, { useContext, useEffect, useState } from 'react';
import { Redirect, useParams } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { ICompetitionTeam } from '../../dto/ICompetitionTeam';
import { IGame } from '../../dto/IGame';
import { IGameAndGameLength } from '../../dto/IGameAndGameLength';
import { IGameLength } from '../../dto/IGameLength';
import { IGameType } from '../../dto/IGameType';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormTwoEntityProps } from '../../types/IFormTwoEnitityProps';
import { IRouteId } from '../../types/IRouteId';

const validation = {
    error: "",
    home: "",
    away: "",
    competition: "",
    gameType: "",
    halfLength: "",
    homeScore: "",
    awayScore: "",
    extraTimeHalfLength: ""
}

const GameEditView = (props: IFormTwoEntityProps<IGame, IGameLength>) => {

    const [gameTypes, setGameTypes] = useState([] as IGameType[]);
    const [competitionTeams, setCompetitionTeams] = useState([] as ICompetitionTeam[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [alertMessage, setAlertMessage] = useState(validation);
    const [added, setSubmitStatus] = useState({ submitStatus: false });
    const appState = useContext(AppContext);

    const loadData = async () => {
        if (props.firstValues.competitionId) {
            let competitionTeamsResult = await BaseService.getAll<ICompetitionTeam>('/CompetitionTeams/competition?competitionId=' + props.firstValues.competitionId, appState.token);
            if (competitionTeamsResult.ok && competitionTeamsResult.data) {
                setCompetitionTeams(competitionTeamsResult.data);
            }
        }
        //let competitionTeamsResult = await BaseService.getAll<ICompetitionTeam>('/CompetitionTeams/competition?competitionId=' + props.firstValues.competitionId, appState.token);
        let gameTypesResult = await BaseService.getAll<IGameType>('/Games/gameTypes', appState.token);

        if (gameTypesResult.ok && gameTypesResult.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setGameTypes(gameTypesResult.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: -1 });
        }
    }

    if (competitionTeams.length === 0) {
        loadData();
    }

    const handleValidation = () => {
        let formIsValid = true;
        setAlertMessage(validation);

        if(props.firstValues.homeId === "-1" || !props.firstValues.homeId) {
            setAlertMessage(prevState => ({
                ...prevState,
                home: "Home team field is required."
            }));
            formIsValid = false;
        }

        if(props.firstValues.awayId === "-1" || !props.firstValues.awayId) {
            setAlertMessage(prevState => ({
                ...prevState,
                away: "Away team field is required."
            }));
            formIsValid = false;
        }

        if(props.firstValues.gameTypeId === "-1" || !props.firstValues.gameTypeId) {
            setAlertMessage(prevState => ({
                ...prevState,
                gameType: "Game type field is required."
            }));
            formIsValid = false;
        }

        if(props.firstValues.homeScore && props.firstValues.homeScore! as unknown as number % 1 !== 0) {
            setAlertMessage(prevState => ({
                ...prevState,
                homeScore: "Score can not be float number."
            }));
            formIsValid = false;
        }

        if(props.firstValues.awayScore && props.firstValues.awayScore! as unknown as number % 1 !== 0) {
            setAlertMessage(prevState => ({
                ...prevState,
                awayScore: "Score can not be float number."
            }));
            formIsValid = false;
        }

        if(!props.secondValues.halfLength || props.secondValues.halfLength === "0" || props.secondValues.halfLength! as unknown as number % 1 !== 0) {
            setAlertMessage(prevState => ({
                ...prevState,
                halfLength: "Half length field is required and can not be 0 or a float number."
            }));
            formIsValid = false;
        }

        if(props.secondValues.extraTimeHalfLength && props.secondValues.extraTimeHalfLength! as unknown as number % 1 !== 0) {
            setAlertMessage(prevState => ({
                ...prevState,
                extraTimeHalfLength: "Extra time half length can not be a float number."
            }));
            formIsValid = false;
        }
        
       return formIsValid;
   }

    const submitClicked = async (e: Event) => {
        e.preventDefault();

        if (!handleValidation()) {
            return;
        };

        if (props.secondValues.extraTimeHalfLength === "") {
            props.secondValues.extraTimeHalfLength = "0";
        }
        
        let response = await BaseService.put<IGameAndGameLength>('/Games/' + props.firstValues.id, { game: props.firstValues, gameLength: props.secondValues }, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else if (response.statusCode === 400) {
            setAlertMessage(prevState => ({
                ...prevState,
                extraTimeHalfLength: 'Extra time length field is required because game type is not "Two halves".'
            }));
        } else {
            setAlertMessage(prevState => ({...prevState, error: "Error code: " + response.statusCode.toString()}));
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps


    if (gameTypes && competitionTeams && props.firstValues.home && props.firstValues.away) {
        return (
            <>
                { appState.token === null ? <Redirect to="/identity/login" /> : null}
                { added.submitStatus === true ? <Redirect to="/games/organiser" /> : null}

                <h4 className="text-center">{props.firstValues.home.name} vs {props.firstValues.away.name}</h4>
                <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
                <hr />
                <p className="redStar">* Field is required</p>
                <div className="row">
                    <div className="col-md-6">
                    <form method="post">
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Game_HomeId">Home team</label> *
                                </div>
                                <select value={props.firstValues.homeId} onChange={(e) => props.handleChange(e.target)} className="form-control" id="Game_HomeId" name="Game.HomeId">
                                    <option value="-1">---Please select---</option>
                                        {competitionTeams.map(compTeam => {
                                            return <option key={compTeam.team!.id} value={compTeam.team!.id!}>{compTeam.team!.name}</option> 
                                        })}
                                </select>
                                {alertMessage.home !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.home}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Game_AwayId">Away team</label> *
                                </div>
                                <select value={props.firstValues.awayId} onChange={(e) => props.handleChange(e.target)} className="form-control" id="Game_AwayId" name="Game.AwayId">
                                    <option value="-1">---Please select---</option>
                                        {competitionTeams.map(compTeam => {
                                            return <option key={compTeam.team!.id} value={compTeam.team!.id!}>{compTeam.team!.name}</option> 
                                        })}
                                </select>
                                {alertMessage.away !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.away}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Game_HomeScore">Home team score</label>
                                <input value={props.firstValues.homeScore !== null ? props.firstValues.homeScore as unknown as number : ""} onChange={(e) => props.handleChange(e.target)} className="form-control" type="number" id="Game_HomeScore" name="Game.HomeScore" />
                                {alertMessage.homeScore !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.homeScore}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Game_AwayScore">Away team score</label>
                                <input value={props.firstValues.awayScore !== null ? props.firstValues.awayScore as unknown as number : ""} onChange={(e) => props.handleChange(e.target)} className="form-control" type="number" id="Game_AwayScore" name="Game.AwayScore" />
                                {alertMessage.awayScore !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.awayScore}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Game_GameTypeId">Game type</label> *
                            </div>
                                <select value={props.firstValues.gameTypeId} onChange={(e) => props.handleChange(e.target)} className="form-control" id="Game_GameTypeId" name="Game.GameTypeId">
                                    <option value="-1">---Please select---</option>
                                        {gameTypes.map(gameType => {
                                            return <option key={gameType.id} value={gameType!.id!}>{gameType.name}</option> 
                                        })}
                                </select>
                                {alertMessage.gameType !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.gameType}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Game_KickOffTime">Kickoff time</label>
                                <input value={props.firstValues.kickOffTime !== null ? props.firstValues.kickOffTime as unknown as string : ""} onChange={(e) => props.handleChange(e.target)} className="form-control" type="datetime-local" id="Game_KickOffTime" name="Game.KickOffTime" />
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="GameLength_HalfLength">Half length (min)</label> *
                                </div>
                                <input value={props.secondValues.halfLength !== null ? props.secondValues.halfLength as unknown as number : ""} onChange={(e) => props.handleChange(e.target)} className="form-control" type="number" data-val="true" data-val-required="Väli Poolaja pikkus (min) on kohustuslik." id="GameLength_HalfLength" name="GameLength.HalfLength" />
                                {alertMessage.halfLength !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.halfLength}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="GameLength_ExtraTimeHalfLength">Extra time half length (min) * (if you did not choose game type to be "Two halves")</label> * (kui Te ei valinud mängu tüübiks "Kaks poolaega")
                                </div>
                                <input value={props.secondValues.extraTimeHalfLength !== null ? props.secondValues.extraTimeHalfLength as unknown as number : ""} onChange={(e) => props.handleChange(e.target)} className="form-control" type="number" id="GameLength_ExtraTimeHalfLength" name="GameLength.ExtraTimeHalfLength" />
                                {alertMessage.extraTimeHalfLength !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.extraTimeHalfLength}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Game_Comment">Comment</label>
                                <textarea value={props.firstValues.comment ?? ""} onChange={(e) => props.handleChange(e.target)} className="form-control" id="Game_Comment" name="Game.Comment"></textarea>
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <input type="submit" onClick={(e) => submitClicked(e.nativeEvent)} value="Edit" className="btn btn-primary" />
                            </div>
                        </form>
                    </div>
                </div>

            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}

const GameEdit = () => {

    const [gameValues, setGameValues] = useState({} as IGame);
    const [gameLengthValues, setGameLengthValues] = useState({} as IGameLength);
    const appState = useContext(AppContext);
    const { id } = useParams() as IRouteId;

    const loadData = async () => {
        let result = await BaseService.get<IGameAndGameLength>('/Games/gameAndGameLength?id=' + id, appState.token!);

        if (result.ok && result.data) {
            setGameValues(result.data.game);
            setGameLengthValues(result.data.gameLength);
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Game_HomeId') {
            setGameValues({ ...gameValues, homeId: target.value });
            return;
        }
        if (target.id === 'Game_AwayId') {
            setGameValues({ ...gameValues, awayId: target.value });
            return;
        }
        if (target.id === 'Game_HomeScore') {
            setGameValues({ ...gameValues, homeScore: target.value as unknown as number });
            return;
        }
        if (target.id === 'Game_AwayScore') {
            setGameValues({ ...gameValues, awayScore: target.value as unknown as number });
            return;
        }
        if (target.id === 'Game_GameTypeId') {
            setGameValues({ ...gameValues, gameTypeId: target.value });
            return;
        }
        if (target.id === 'Game_KickOffTime') {
            setGameValues({ ...gameValues, kickOffTime: target.value as unknown as Date });
            return;
        }
        if (target.id === 'GameLength_HalfLength') {
            setGameLengthValues({ ...gameLengthValues, halfLength: target.value });
            return;
        }
        if (target.id === 'GameLength_ExtraTimeHalfLength') {
            setGameLengthValues({ ...gameLengthValues, extraTimeHalfLength: target.value });
            return;
        }
        if (target.id === 'Game_Comment') {
            setGameValues({ ...gameValues, comment: target.value });
            return;
        }
    }

    return <GameEditView firstValues={gameValues} secondValues={gameLengthValues} handleChange={handleChange} />
};


export default GameEdit;