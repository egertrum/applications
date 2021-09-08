import { useContext } from "react";
import Dropdown from "react-bootstrap/Dropdown";
import { Link } from "react-router-dom";
import { AppContext } from "../context/AppContext";
import database from '../wwwroot/images/database.png';
import reports from '../wwwroot/images/report.png';
import competition from '../wwwroot/images/competition.png';
import user from '../wwwroot/images/user.png';


const Navbar = () => {
    const appState = useContext(AppContext);
    return (
        <nav className="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">

            <ul className="navbar-nav">
                {appState.token === null ?
                    <>
                        <li className="nav-item">
                            <Link className="nav-link text-dark" to="/identity/login">Login</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link text-dark" to="/identity/register">Register</Link>
                        </li>
                    </>
                    :
                    <>
                        <li className="nav-item">
                            <Link to="/identity/profile">
                                <span className="nav-link text-dark">
                                    <img className="very-small-icon" src={user} alt="" />
                            Hello, {appState.firstName + ' ' + appState.lastName}!</span>
                            </Link>
                        </li>
                        <li className="nav-item">
                            <button onClick={() => appState.setAuthInfo(null, '', '', '')}  className="btn btn-link nav-link text-dark" >Logout</button>
                        </li>
                    </>
                }
            </ul>

            {appState.role === "FootyUser" ?
                <>
                    <div className="topbar-divider d-none d-sm-block"></div>
                    <Dropdown>
                        <Dropdown.Toggle variant="success" id="dropdown-basic">
                            <img className="very-small-icon" src={database} alt="" />
                            My Teams
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                            <Dropdown.Header>Existing:</Dropdown.Header>
                            <Link className="dropdown-item" to="/teams">
                                Teams
                            </Link>
                            <Link className="dropdown-item" to="/competitionTeams/team">
                                Attending competitions
                            </Link>
                            <Link className="dropdown-item" to="/games/team">
                                Games
                            </Link>
                            <Link className="dropdown-item" to="/persons">
                                Members
                            </Link>
                            <Dropdown.Header>Add new:</Dropdown.Header>
                            <Link className="dropdown-item" to="/teams/create">
                                Team
                            </Link>
                            <Link className="dropdown-item" to="/persons/create">
                                Member
                            </Link>
                            <Link className="dropdown-item" to="/teamPersons/create">
                                Member to Team
                            </Link>
                        </Dropdown.Menu>
                    </Dropdown>
                    <div className="topbar-divider d-none d-sm-block"></div>
                    <Dropdown>
                        <Dropdown.Toggle variant="success" id="dropdown-basic">
                            <img className="very-small-icon" src={competition} alt="" />
                            My Competitions
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <Link className="dropdown-item" to="/myCompetitions">
                                Competitions
                                </Link>
                            <Link className="dropdown-item" to="/games/organiser">
                                Games
                                </Link>
                            <Link className="dropdown-item" to="/competitionTeams/organiser">
                                Participations
                                </Link>
                        </Dropdown.Menu>
                    </Dropdown>
                </>
                : null
            }

            {appState.role === "Admin" ?
                <>
                    <div className="topbar-divider d-none d-sm-block"></div>
                    <Dropdown>
                        <Dropdown.Toggle variant="success" id="dropdown-basic">
                            <img className="very-small-icon" src={database} alt="" />
                            DATABASE
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                            <Dropdown.Item>Users</Dropdown.Item>
                            <Dropdown.Item>Roles</Dropdown.Item>
                            <Dropdown.Divider />
                            <Dropdown.Item>Competitions</Dropdown.Item>
                            <Dropdown.Item>CompetitionTeams</Dropdown.Item>
                            <Dropdown.Divider />
                            <Dropdown.Item>GameParts</Dropdown.Item>
                            <Dropdown.Item>GamePartTypes</Dropdown.Item>
                            <Dropdown.Item>GamePersons</Dropdown.Item>
                            <Link className="dropdown-item" to="/games">
                                Games
                            </Link>
                            <Dropdown.Divider />
                            <Dropdown.Item>Events</Dropdown.Item>
                            <Link className="dropdown-item" to="/countries">
                                Countries
                            </Link>
                            <Dropdown.Item>Participations</Dropdown.Item>
                            <Dropdown.Item>PersonRoles</Dropdown.Item>
                            <Dropdown.Item>Persons</Dropdown.Item>
                            <Dropdown.Item>Registrations</Dropdown.Item>
                            <Dropdown.Item>TeamPersons</Dropdown.Item>
                            <Link className="dropdown-item" to="/teams">
                                Teams
                            </Link>
                        </Dropdown.Menu>
                    </Dropdown>

                    <div className="topbar-divider d-none d-sm-block"></div>

                    <li className="nav-item text-dark">
                        <Link className="nav-link text-dark" to="/Reports">
                            <img className="very-small-icon" src={reports} alt="" />
                                REPORTS
                        </Link>
                    </li>
                </>
                : null
            }
        </nav>
    );
}

export default Navbar;