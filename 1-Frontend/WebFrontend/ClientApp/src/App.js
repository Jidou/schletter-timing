import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Race } from './components/Race';
import { Groups } from './components/Groups';
import { Participants } from './components/Participants';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/race' component={Race} />
            <Route path='/groups' component={Groups} />
            <Route path='/participants' component={Participants} />
      </Layout>
    );
  }
}
