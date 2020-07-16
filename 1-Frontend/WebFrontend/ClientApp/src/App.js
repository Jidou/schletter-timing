import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Race } from './components/race/Race';
import { RaceGroups } from './components/race/RaceGroups';
import { RaceParticipants } from './components/race/RaceParticipants';
import { RaceOverview } from './components/RaceOverview';
import { Groups } from './components/Groups';
import { Participants } from './components/Participants';
import 'bootstrap/dist/css/bootstrap.min.css';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/race/race' component={Race} />
        <Route path='/race/racegroups' component={RaceGroups} />
        {/* <Route path='/race/raceparticipants' component={RaceParticipants} /> */}
        <Route path='/raceoverview' component={RaceOverview} />
        <Route path='/groups' component={Groups} />
        <Route path='/participants' component={Participants} />
      </Layout>
    );
  }
}
