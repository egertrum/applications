import { Redirect } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { AppContext } from "../../context/AppContext";
import { IAppUser } from "../../dto/IAppUser";


const Profile = () => {

    const [user, setUser] = useState({} as IAppUser);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const appState = useContext(AppContext);

    useEffect(() => {
        const loadData = async () => {
            let result = await BaseService.get<IAppUser>('/Account/GetUserInfo', appState.token!);

            if (result.ok && result.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setUser(result.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (appState.token === null) {
        return <><Redirect to={"/identity/login"} /></>
    }

    if (user) {
        return (
            <>
                <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-9">
                        <div className="row">
                            <div>
                            <h4>Profile</h4>
                                <dt>
                                    First name
                                </dt>
                                <dd>
                                    {user.firstname}
                                </dd>
                                <dt>
                                    Last name
                                </dt>
                                <dd>
                                    {user.lastname}
                                </dd>
                                <dt>
                                    Email
                                </dt>
                                <dd>
                                    {user.email}
                                </dd>
                                <dt>
                                    Identification code
                                </dt>
                                <dd>
                                    {user.identificationCode}
                                </dd>
                                <dt>
                                    Role
                                </dt>
                                <dd>
                                    {appState.role}
                                </dd>
                            </div>
                        </div>
                    </div>
                </div>
            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}

export default Profile;