import { useContext, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Loader from "../../components/Loader";
import { AppContext } from "../../context/AppContext";
import { ICompetition } from "../../dto/ICompetition";
import { ICompetitionSearch } from "../../dto/ICompetitionSearch";
import { ICountry } from "../../dto/ICountry";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { IFormProps } from "../../types/IFormProps";
import { IRouteId } from "../../types/IRouteId";
import pin from '../../wwwroot/images/pin.png';
import arrow from '../../wwwroot/images/right-arrow.png';

const initialFormValues: ICompetitionSearch = {
    countryId: '',
    name: '',
    startDate: '' as unknown as Date,
};

const RowDisplay = (props: { competition: ICompetition, myComps: boolean }) => {

    const appState = useContext(AppContext);
    var dateformat = require("dateformat");
    let tilDate = props.competition.endDate ? <> - {dateformat(props.competition.endDate, "dd/mm/yyyy")}</> : null

    return (
        <tr>
            <td>
                {props.competition.name}
            </td>
            <td>
                <img className="very-small-icon" src={pin} alt="" />
                {props.competition.country!.name}
            </td>
            <td>
                {dateformat(props.competition.startDate, "dd/mm/yyyy")}{tilDate}
            </td>
            <td>
                {appState.token !== null ?
                    <>
                    <Link className="text-dark" to={'/competitionTeams/create/' + props.competition.id}>Register</Link>
                    |
                    </> : null}
                <Link className="text-dark" to={'/competitions/' + props.competition.id}>More Info</Link>
                {appState.role === "Admin" || props.myComps === true ?
                    <>
                    |
                    <Link className="text-dark" to={'/games/create/' + props.competition.id}>Add game</Link>
                    |
                    <Link className="text-dark" to={'/competitions/edit/' + props.competition.id}>Edit</Link>
                    |
                    <Link className="text-dark" to={'/competitions/delete/' + props.competition.id}>Delete</Link>
                    </> : null}
            </td>
        </tr>
    );
}

const CompetitionIndexView = (props: IFormProps<ICompetitionSearch>) => {
    const [competitions, setCompetitions] = useState([] as ICompetition[]);
    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [myComps] = useState({ myCompsSet: false });
    const [title, setTitle] = useState({ name: "Competitions"} );
    const appState = useContext(AppContext);
    const {id} = useParams() as IRouteId;

    const loadData = async (searchCompetition?: ICompetitionSearch) => {
        let competitionsUri = '/Competitions';
        if (searchCompetition) {
            competitionsUri += "/search" +
                "?countryId=" + searchCompetition.countryId +
                "&name=" + searchCompetition.name +
                "&startDate=" + searchCompetition.startDate;
            }

        if (myComps.myCompsSet) {
            competitionsUri += "/myCompetitions";
            setTitle({ name: "My competitions" });
        }

        let result = await BaseService.getAll<ICompetition>(competitionsUri, appState.token);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCompetitions(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    const getSearchCompetition = (e: Event) => {
        e.preventDefault();
        loadData(props.values as ICompetitionSearch);
    }

    const myCompetitions = () => {
        myComps.myCompsSet = true;
        loadData();
    }

    if (id && id === "myCompetitions" && myComps.myCompsSet === false) {
        myCompetitions();
    }

    const resetCompetitions = () => {
        props.values.countryId = '';
        props.values.name = '';
        props.values.startDate = '' as unknown as Date;
        myComps.myCompsSet = false;
        loadData();
    }

    const loadCountries = async () => {
        let result = await BaseService.getAll<ICountry>("/Countries");
        if (result.ok && result.data) {
            setCountries(result.data);
        }
    }

    useEffect(() => {
        loadCountries();
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (

        <>
            <h4 className="text-center">{title.name}</h4>

            {myComps.myCompsSet === true ?
                <>
                    <p className="text-center">
                        <Link className="text-dark" to="" onClick={resetCompetitions}>Back to all competitions
                        <img className="extra-small-icon" src={arrow} alt="" /></Link>
                    </p>
                </> : null
            }
            {myComps.myCompsSet === false && appState.token !== null ?
                <>
                    <p className="text-center">
                        <Link className="text-dark" to="" onClick={myCompetitions}>Competitions organised by me
                            <img className="extra-small-icon" src={arrow} alt="" />
                        </Link>
                    </p>
                </> : null
            }
            {myComps.myCompsSet === false ?
                <>
                    <hr />
                    <form method="get" onSubmit={(e) => getSearchCompetition(e.nativeEvent)}>
                        <div className="form-group">
                            <div className="row">
                                <div className="col">
                                    <label className="control-label" htmlFor="Competition_CountryId">Country</label>
                                    <select value={props.values.countryId} onChange={(e) => props.handleChange(e.target)} name="countryId" className="form-control" id="Competition_CountryId">
                                        <option></option>
                                        {countries.map(country => {
                                            return <option key={country.id} value={country!.id!}>{country.name}</option>
                                        })}
                                    </select>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label className="control-label" htmlFor="Competition_Name">Competition name</label>
                                        <input value={props.values.name} onChange={(e) => props.handleChange(e.target)} name="name" className="form-control" type="text" id="Competition_Name" maxLength={256} />
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label className="control-label" htmlFor="Competition_StartDate">Starting after:</label>
                                        <input value={props.values.startDate as unknown as string} onChange={(e) => props.handleChange(e.target)} className="form-control" type="date" id="Competition_StartDate" name="Competition.StartDate" />
                                    </div>
                                </div>
                            </div>
                            <div className="text-center">
                                <div className="form-group">
                                    <input type="submit" value="Search" className="btn btn-primary" />
                                    <Link to="" type="submit" onClick={resetCompetitions} className="btn btn-primary">Reset</Link>
                                </div>
                            </div>
                        </div>
                    </form>
                </>
                :
                null
            }
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                            Competition name
                        </th>
                        <th>
                        </th>
                        <th>
                            Date
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {competitions.map(competition =>
                        <RowDisplay competition={competition} myComps={myComps.myCompsSet} key={competition.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

const CompetitionIndex = () => {

    const [formValues, setFormValues] = useState(initialFormValues);

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Competition_CountryId') {
            setFormValues({ ...formValues, countryId: target.value });
            return;
        }
        if (target.id === 'Competition_Name') {
            setFormValues({ ...formValues, name: target.value });
            return;
        }
        if (target.id === 'Competition_StartDate') {
            setFormValues({ ...formValues, startDate: target.value as unknown as Date });
            return;
        }
    }
    return <CompetitionIndexView values={formValues} handleChange={handleChange} />
};

export default CompetitionIndex;
