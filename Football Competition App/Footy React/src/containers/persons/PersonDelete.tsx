import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useEffect, useState, useContext } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { Redirect } from "react-router";
import { AppContext } from "../../context/AppContext";
import { EDelete } from "../../types/EDelete";
import { IPerson } from "../../dto/IPerson";


const PersonDelete = () => {

    const { id } = useParams() as IRouteId;
    const [person, setPerson] = useState({} as IPerson);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });
    const appState = useContext(AppContext);
    var dateformat = require("dateformat");

    useEffect(() => {
        const loadData = async () => {
            let personResult = await BaseService.get<IPerson>('/Persons/' + id, appState.token!);

            if (personResult.ok && personResult.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setPerson(personResult.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: personResult.statusCode });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<IPerson>('/Persons/' + id, appState.token!);
        if (response.ok) {
            setDelete({deleteStatus: EDelete.Deleted});
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: response.statusCode });
        }
    }

    if (person && person.country) {
        return (
            <>
            { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/persons" /> : null}
                <form onSubmit={(e) => deleteClicked(e.nativeEvent)}>
                    <h4 className="text-center">Are you sure you want to delete this?</h4>
                    <h5 className="text-center">{person.firstName} {person.lastName}</h5>
                    
                    { pageStatus.statusCode === 400 ? 
                        <>
                            <div className="errorClass">Can not delete this person! Try to remove this person from your team(s)
                            and then delete this person. 
                            If Fails then it means that this person is registered at different events and can not be deleted.
                            </div>
                        </> 
                        : null
                    }
                    
                    <div>
                        <hr />
                        <dl className="row">
                            <dt className="col-sm-4">
                                Birth country
                            </dt>
                            <dd className="col-sm-6">
                                {person.country.name}
                            </dd>
                            <dt className="col-sm-4">
                                First name
                            </dt>
                            <dd className="col-sm-6">
                                {person.firstName}
                            </dd>
                            <dt className="col-sm-4">
                                Last name
                            </dt>
                            <dd className="col-sm-6">
                                {person.lastName}
                            </dd>
                            <dt className="col-sm-4">
                                Teams associated with
                            </dt>
                            <dd className="col-sm-6">
                                {Object.entries(person.personTeams!).map(([team, games], index) => {
                                    if (index !== 0) {
                                        return <span key={index}> | {team}</span>
                                    } 
                                    return <span key={index}>{team}</span>
                                })}
                            </dd>
                            <dt className="col-sm-4">
                                Date of birth
                            </dt>
                            <dd className="col-sm-6">
                                {dateformat(person.birthDate, "dd/mm/yyyy")}
                            </dd>
                            <dt className="col-sm-4">
                                Gender
                            </dt>
                            <dd className="col-sm-6">
                                {person.gender}
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
        <Loader {...pageStatus} />
    );
}

export default PersonDelete;