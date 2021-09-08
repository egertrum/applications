import { useContext, useEffect, useState } from "react";
import { Link, Redirect } from "react-router-dom";
import Loader from "../../components/Loader";
import { AppContext } from "../../context/AppContext";
import { IPerson } from "../../dto/IPerson";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import pin from '../../wwwroot/images/pin.png';

const RowDisplay = (props: { person: IPerson }) => {

    var dateformat = require("dateformat");

    return (
        <tr>
            <td>
                <img className="very-small-icon" src={pin} alt="" />
                {props.person.country!.name}
            </td>
            <td>
                {props.person.firstName}
            </td>
            <td>
                {props.person.lastName}
            </td>
            <td>
                {props.person.identificationCode}
            </td>
            <td>
                {dateformat(props.person.birthDate, "dd/mm/yyyy")}
            </td>
            <td>
                {props.person.gender}
            </td>
            <td>
                <Link className="text-dark" to={'/persons/' + props.person.id}>More Info</Link>
                |
                <Link className="text-dark" to={'/persons/edit/' + props.person.id}>Edit</Link>
                |
                <Link className="text-dark" to={'/persons/delete/' + props.person.id}>Delete</Link>
            </td>
        </tr>
    );
}

const PersonIndex = () => {

    const [persons, setPersons] = useState([] as IPerson[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const appState = useContext(AppContext);

    const loadData = async () => {
        let personsUri = '/Persons';
        let result = await BaseService.getAll<IPerson>(personsUri, appState.token);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setPersons(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (

        <>
            { appState.token === null ? <Redirect to="/identity/login" /> : null}

            <h4 className="text-center">Members</h4>

            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                            First name
                        </th>
                        <th>
                            Last name
                        </th>
                        <th>
                            Identification code
                        </th>
                        <th>
                            Date of birth
                        </th>
                        <th>
                            Gender
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {persons.map(person =>
                        <RowDisplay person={person} key={person.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

export default PersonIndex;