import { useState } from 'react';
import { Route, Switch } from 'react-router-dom';
import { AppContextProvider, initialAppState } from './context/AppContext';

// Layout
import Footer from './components/Footer';
import Navbar from './components/Navbar';
import Sidebar from './components/Sidebar';

// Container views
import CompetitionCreate from './containers/competitions/CompetitionCreate';
import CompetitionDelete from './containers/competitions/CompetitionDelete';
import CompetitionDetails from './containers/competitions/CompetitionDetails';
import CompetitionEdit from './containers/competitions/CompetitionEdit';
import CompetitionIndex from './containers/competitions/CompetitionIndex';
import CompetitionTeamDelete from './containers/competitionTeams/CompetitionTeamDelete';
import CompetitionTeamIndex from './containers/competitionTeams/CompetitionTeamIndex';
import CompetitionTeamsCreate from './containers/competitionTeams/CompetitionTeamsCreate';
import CountryCreate from './containers/countries/CountryCreate';
import CountryDelete from './containers/countries/CountryDelete';
import CountryIndex from './containers/countries/CountryIndex';
import GameCreate from './containers/games/GameCreate';
import GameDelete from './containers/games/GameDelete';
import GameEdit from './containers/games/GameEdit';
import GameIndex from './containers/games/GameIndex';
import Login from './containers/identity/Login';
import Register from './containers/identity/Register';
import Page404 from './containers/Page404';
import PersonCreate from './containers/persons/PersonCreate';
import PersonDelete from './containers/persons/PersonDelete';
import PersonDetails from './containers/persons/PersonDetails';
import PersonEdit from './containers/persons/PersonEdit';
import PersonIndex from './containers/persons/PersonIndex';
import ReportCreate from './containers/reports/ReportCreate';
import ReportIndex from './containers/reports/ReportIndex';
import TeamPersonCreate from './containers/teamPersons/TeamPersonCreate';
import TeamPersonDelete from './containers/teamPersons/TeamPersonDelete';
import TeamCreate from './containers/teams/TeamCreate';
import TeamDelete from './containers/teams/TeamDelete';
import TeamEdit from './containers/teams/TeamEdit';
import TeamsDetails from './containers/teams/TeamsDetails';
import TeamsIndex from './containers/teams/TeamsIndex';
import Profile from './containers/identity/Profile';

function App() {
  const setAuthInfo = (token: string | null, firstName: string, lastName: string, role:string): void => {
    setAppState({ ...appState, token, firstName, lastName, role });
  }

  const [appState, setAppState] = useState({ ...initialAppState, setAuthInfo });

  return (
        <AppContextProvider value={appState} >
        <div id="wrapper">
          <Sidebar />
          <div id="content-wrapper" className="d-flex flex-column">
            <div id="content">
              <Navbar />
              <div className="container-fluid">
                <main role="main" className="pb-3">
                  <Switch>
                    
                    <Route path="/identity/login" component={Login} />
                    <Route path="/identity/register" component={Register} />
                    <Route path="/identity/profile" component={Profile} />

                    <Route path="/competitions/create" component={CompetitionCreate} />
                    <Route path="/competitions/edit/:id" component={CompetitionEdit} />
                    <Route path="/competitions/delete/:id" component={CompetitionDelete} />
                    <Route exact path="/competitions/:id" component={CompetitionDetails} />
                    <Route exact path="/competitions/:countryId?/:name?/:startDate?" component={CompetitionIndex} />

                    <Route path="/competitionTeams/create/:id?" component={CompetitionTeamsCreate} />
                    <Route path="/competitionTeams/delete/:id?" component={CompetitionTeamDelete} />
                    <Route path="/competitionTeams/:id?" component={CompetitionTeamIndex} />

                    <Route path="/teams/create" component={TeamCreate} />
                    <Route path="/teams/edit/:id" component={TeamEdit} />
                    <Route path="/teams/delete/:id" component={TeamDelete} />
                    <Route path="/teams/:id" component={TeamsDetails} />
                    <Route exact path="/teams" component={TeamsIndex} />

                    <Route exact path="/teamPersons/create/id=:id?" component={TeamPersonCreate} />
                    <Route exact path="/teamPersons/create/person=:person?" component={TeamPersonCreate} />
                    <Route exact path="/teamPersons/create" component={TeamPersonCreate} />
                    <Route path="/teamPersons/delete/:id" component={TeamPersonDelete} />

                    <Route path="/persons/create" component={PersonCreate} />
                    <Route path="/persons/edit/:id" component={PersonEdit} />
                    <Route path="/persons/delete/:id" component={PersonDelete} />
                    <Route path="/persons/:id" component={PersonDetails} />
                    <Route exact path="/persons" component={PersonIndex} />

                    <Route path="/games/create/:id" component={GameCreate} />
                    <Route path="/games/delete/:id" component={GameDelete} />
                    <Route path="/games/edit/:id" component={GameEdit} />
                    <Route exact path="/games/:id?" component={GameIndex} />

                    <Route path="/countries/create" component={CountryCreate} />
                    <Route path="/countries/delete/:id" component={CountryDelete} />
                    <Route exact path="/countries" component={CountryIndex} />

                    <Route path="/reports/create" component={ReportCreate} />
                    <Route path="/reports/delete/:id" component={CountryDelete} />
                    <Route exact path="/reports" component={ReportIndex} />

                    <Route exact path="/:id?" component={CompetitionIndex}/>

                    <Route component={Page404} />
                  </Switch>
                </main>
              </div>
            </div>
            <Footer />
          </div>
        </div>
        <script src="/lib/jquery/dist/jquery.min.js"></script>
        <script src="/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
        <script src="~/lib/jquery/jquery-easing/jquery.easing.min.js"></script>
        </AppContextProvider>
  );
}

export default App;