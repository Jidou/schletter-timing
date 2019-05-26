import React, { Component } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Race extends Component {
    static displayName = Race.name;

    dirty = false;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleLoad = this.handleLoad.bind(this);
        this.state = { race: [], loading: true };

        fetch('api/Race/')
            .then(response => response.json())
            .then(data => {
                this.setState({ race: data, loading: false });
            });
    }


    renderRaceForm(race) {
        return (
            <div>
                <div className="form-group">
                    <label for="Titel">Racename</label>
                    <input type="text" className="form-control" id="Titel" onChange={this.handleChange} value={this.state.race.titel}></input>
                </div>
                <div className="form-group">
                    <label for="RaceType">RaceType</label>
                    <input type="text" className="form-control" id="RaceType" onChange={this.handleChange} placeholder="Some name" value={this.state.race.raceType} />
                </div>
                <div className="form-group">
                    <label for="Place">Place</label>
                    <input type="text" className="form-control" id="Place" onChange={this.handleChange} placeholder="Some name" value={this.state.race.place} />
                </div>
                <div className="form-group">
                    <label for="Date">Date</label>
                    <input type="date" className="form-control" id="Date" onChange={this.handleChange} placeholder="2019-05-26" value={this.state.race.date} />
                </div>
                <div className="form-group">
                    <label for="Judge">Judge</label>
                    <input type="text" className="form-control" id="Judge" onChange={this.handleChange} placeholder="Some name" value={this.state.race.judge} />
                </div>
                <div className="form-group">
                    <label for="TimingTool">TimingTool</label>
                    <input type="text" className="form-control" id="TimingTool" onChange={this.handleChange} placeholder="AlgeTiming" value={this.state.race.timingTool} />
                </div>
                <button type="submit" className="btn btn-primary">Save</button>
                <button type="button" onClick={this.handleLoad} disabled={this.dirty} className="btn btn-primary">Load</button>
                <button type="button" onClick={this.handleAddGroup} disabled={this.dirty} className="btn btn-primary">Add Group</button>
                <button type="button" onClick={this.handleAddParticipant} disabled={this.dirty} className="btn btn-primary">Add Participant</button>
            </div>
        );
    }


    handleChange(event) {
        var tmp = this.state.race;
        var target = event.target.id;
        var value = event.target.value;

        if (target === "Titel") {
            tmp.titel = value;
        } else if (target === "RaceType") {
            tmp.raceType = value;
        } else if (target === "Place") {
            tmp.place = value;
        } else if (target === "Date") {
            tmp.date = value;
        } else if (target === "TimingTool") {
            tmp.timingTool = value;
        } else if (target === "Judge") {
            tmp.judge = value;
        } else {
            return;
        }

        this.setState({
            race: tmp
        });

        this.dirty = true;
    }


    handleSubmit(event) {
        fetch('api/Race/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.race)
        });

        event.preventDefault();
        toast("Race saved successfully");
        this.dirty = false;
    }


    handleLoad() {
        fetch('api/Race/Load/')
            .then(response => response.json())
            .then(data => {
                this.setState({ race: data, loading: false });
            });
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderRaceForm(this.state.race);

        return (
            <div>
                <h1>Race</h1>
                <form onSubmit={this.handleSubmit}>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
