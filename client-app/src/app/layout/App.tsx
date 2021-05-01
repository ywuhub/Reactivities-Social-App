import React from 'react';
import { Container } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { Route, useLocation } from 'react-router-dom';
import HomePage from '../../features/home/homepage';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import ActivityForm from '../../features/activities/form/ActivityForm';
import NavBar from './NavBar';
import ActivityDetails from '../../features/activities/details/ActivityDetails';

function App() {

  const location = useLocation();

  return (
    <>
    <div className="App">
      <NavBar/>
      <Container style={{marginTop: '7em'}}>
        <Route exact path='/' component={HomePage} />
        <Route exact path='/activities' component={ActivityDashboard} />
        <Route path='/activities/:id' component={ActivityDetails} />
        <Route key={location.key} path={['/createActivity', '/manage/:id']} component={ActivityForm} />
      </Container>
    </div>
    </>
  );
}

export default observer(App);
