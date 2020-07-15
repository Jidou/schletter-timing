import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { RaceOverview } from './components/race/Overview';
import { RaceGroups } from './components/race/Groups';
import { RaceParticipants } from './components/race/Participants';
import { Races } from './components/Races';
import { Groups } from './components/Groups';
import { Participants } from './components/Participants';
import 'bootstrap/dist/css/bootstrap.min.css';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/race/overview' component={RaceOverview} />
        <Route path='/race/groups' component={RaceGroups} />
        <Route path='/race/participants' component={RaceParticipants} />
        <Route path='/races' component={Races} />
        <Route path='/groups' component={Groups} />
        <Route path='/participants' component={Participants} />
      </Layout>
    );
  }
}
