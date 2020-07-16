import React, { Component } from 'react';
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
                <table className="table table-striped table-hover">
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
                </table>

                {/* <div>
                    <nav aria-label="Page navigation example">
                        <ul className="pagination">
                            <li className="page-item"><a className="page-link" href="#">Previous</a></li>
                            <li className="page-item"><a className="page-link" href="#">1</a></li>
                            <li className="page-item"><a className="page-link" href="#">2</a></li>
                            <li className="page-item"><a className="page-link" href="#">3</a></li>
                            <li className="page-item"><a className="page-link" href="#">Next</a></li>
                        </ul>
                    </nav>
                </div> */}
            </div>
        );
    }


    handleSubmit(event) {
        fetch('api/RaceOverview/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify("")
        })
        .then(response => response.status)
        .then(this.props.history.push("/race/race"));
    }


    handleLoad(racename, event) {
        fetch('api/RaceOverview/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(racename)
        })
        .then(response => response.status)
        .then(this.props.history.push("/race/race"));

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
