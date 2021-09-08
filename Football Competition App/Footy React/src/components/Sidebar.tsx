import { Link } from "react-router-dom";
import logo from '../wwwroot/images/cup.png';

const Sidebar = () => (
  <ul className="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">

    <Link className="sidebar-brand d-flex align-items-center justify-content-center" to={'/'}>
      <div className="sidebar-brand-icon">
        <img className="small-icon" src={logo} alt={""} />
      </div>
      <div className="sidebar-brand-text mx-3">Footy <sup></sup></div>
    </Link>

    <hr className="sidebar-divider my-0" />

    <li className="nav-item">
      <Link className="nav-link" to={'/'}>
        <i className="fas fa-fw fa-chart-area"></i>
        <span>Competitions</span>
      </Link>
    </li>

    <li className="nav-item">
      <Link className="nav-link" to={'/games'}>
        <i className="fas fa-fw fa-chart-area"></i>
        <span>Fixtures</span>
      </Link>
    </li>

    <li className="nav-item">
      <div className="nav-link collapsed" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
        <i className="fas fa-fw fa-cog"></i>
        <span>Statistics</span>
      </div>
      <div id="collapseTwo" className="collapse" aria-labelledby="headingTwo" data-parent="#accordionSidebar">
        <div className="bg-white py-2 collapse-inner rounded">
          <div className="collapse-item">Teams</div>
          <div className="collapse-item">Players</div>
        </div>
      </div>
    </li>

    <hr className="sidebar-divider" />

    <div className="sidebar-heading">
      Contact
    </div>

    <li className="nav-item">
      <Link className="nav-link" to={'/competitions/create'}>
        <i className="fas fa-fw fa-chart-area"></i>
        <span>Submit your Competition</span>
      </Link>
    </li>

    <li className="nav-item">
    <Link className="nav-link" to={'/competitionTeams/create'}>
        <i className="fas fa-fw fa-chart-area"></i>
        <span>Register to Competition</span>
      </Link>
    </li>

    <li className="nav-item">
      <Link className="nav-link" to={'/reports/create'}>
        <i className="fas fa-fw fa-chart-area"></i>
        <span>Report a Problem</span>
      </Link>
    </li>

    <hr className="sidebar-divider" />
  </ul>
);

export default Sidebar;