import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import { SortableContainer, SortableElement } from 'react-sortable-hoc';
import arrayMove from 'array-move';
import Autosuggest from 'react-autosuggest';

import 'react-toastify/dist/ReactToastify.css';


export class RaceOverview extends Component {
    static displayName = RaceOverview.name;

    dirty = false;

    constructor(props) {
        super(props);
        this.handleLoad = this.handleLoad.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        this.state = { races: [], loading: true };

        fetch('api/RaceOverview/')
            .then(response => response.json())
            .then(data => {
                this.setState({ races: data, loading: false });
            });
    }


    renderOverviewTable(races) {
        return (
            <div>
                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Racename</th>
                            <th>Date</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {races.map(race =>
                            <tr key={race.name}>
                                <td>{race.name}</td>
                                <td>{race.date}</td>
                                <td>
                                    <button type="button" className="btn btn-primary" onClick={this.handleLoad.bind(this, race.name)}>Load</button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }


    handleSubmit(event) {
        this.props.history.push("/race/newrace");
    }


    handleLoad(racename, event) {
        fetch('api/Race/SetCurrentRace?racename=' + racename)
            .then(response => response.json())
            .then(_ => {
                this.props.history.push("/race/race/");
            });
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderOverviewTable(this.state.races);

        return (
            <div>
                <h1>Race Overview</h1>
                <form onSubmit={this.handleSubmit}>
                    <button type="submit" className="btn btn-primary">New Race</button>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
