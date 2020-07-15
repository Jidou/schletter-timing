import React, { Component } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Autosuggest from 'react-autosuggest';


export class RaceParticipants extends Component {
    static displayName = RaceParticipants.name;

    newParticipantsCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleAddParticipant = this.handleAddParticipant.bind(this);
        this.onSuggestionsFetchRequested = this.onSuggestionsFetchRequested.bind(this);
        this.onSuggestionsClearRequested = this.onSuggestionsClearRequested.bind(this);
        this.onChange = this.onChange.bind(this);
        this.getSuggestionValue = this.getSuggestionValue.bind(this);
        this.getInputProps = this.getInputProps.bind(this);
        this.state = { groups: [], participants: [], suggestions: [], value: "", loading: true };

        fetch('api/Group/GetIdAndNameOnly/')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data, suggestions: data });
            });

        fetch('api/Participant/')
            .then(response => response.json())
            .then(data => {
                this.setState({ participants: data, loading: false, activePage: 1 });
            });
    }


    handleChange(participantId, event) {
        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var target = event.target.id;
        var value = event.target.value;
        var tmp = participants[index];

        if (target === "Firstname") {
            tmp.firstname = value;
        } else if (target === "Lastname") {
            tmp.lastname = value;
        } else if (target === "Category") {
            tmp.category = value;
        } else if (target === "YearOfBirth") {
            tmp.yearOfBirth = value;
        } else {
            return;
        }

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        participants[index] = tmp;

        this.setState({
            participants: participants
        });
    }


    handleAddParticipant() {
        var newParticipant = {
            participantId: this.newParticipantsCounter,
            firstname: "",
            lastname: "",
            category: "",
            yearOfBirth: "",
            groupId: 0,
            groupName: "",
            toAdd: true,
        }

        this.newParticipantsCounter--;

        var participants = this.state.participants;
        participants.push(newParticipant);

        this.setState({
            participants: participants
        });
    }


    handleSubmit(event) {
        fetch('api/Participant/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.participants)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ participants: data });
            });

        event.preventDefault();
        toast("Participants saved successfully");
    }


    renderParticipantsTable(participants) {
        return (
            <div>
                <table className="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>Firstname</th>
                            <th>Lastname</th>
                            <th>Group</th>
                            <th>Category</th>
                            <th>Year Of Birth</th>
                        </tr>
                    </thead>
                    <tbody>
                        {participants.map(participant =>
                            <tr key={participant.participantId}>
                                <td>
                                    <input type="text" id="Firstname" onChange={this.handleChange.bind(this, participant.participantId)} value={participant.firstname}></input>
                                </td>
                                <td>
                                    <input type="text" id="Lastname" onChange={this.handleChange.bind(this, participant.participantId)} value={participant.lastname}></input>
                                </td>
                                <td>
                                    <Autosuggest
                                        suggestions={this.state.suggestions}
                                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                                        getSuggestionValue={this.getSuggestionValue.bind(this, participant.participantId)}
                                        renderSuggestion={this.renderSuggestion}
                                        inputProps={this.getInputProps(participant.participantId)}
                                    />
                                </td>
                                <td>
                                    <input type="text" id="Category" onChange={this.handleChange.bind(this, participant.participantId)} value={participant.category}></input>
                                </td>
                                <td>
                                    <input type="text" id="YearOfBirth" onChange={this.handleChange.bind(this, participant.participantId)} value={participant.yearOfBirth}></input>
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


    getInputProps(participantId) {
        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var tmp = participants[index].groupName;

        if (tmp === null) {
            tmp = "";
        }

        return {
            placeholder: "Groupname",
            value: tmp,
            onChange: this.onChange.bind(this, participantId)
        };
    }


    getSuggestions = value => {
        const inputValue = value.trim().toLowerCase();
        const inputLength = inputValue.length;

        return inputLength === 0 ? [] : this.state.groups.filter(group =>
            group.groupname.toLowerCase().slice(0, inputLength) === inputValue
        );
    };


    onSuggestionsFetchRequested = ({ value }) => {
        this.setState({
            suggestions: this.getSuggestions(value)
        });
    };


    onSuggestionsClearRequested = () => {
        this.setState({
            suggestions: this.state.groups,
        });
    };


    getSuggestionValue(participantId, suggestion) {
        var groups = this.state.groups;
        var group = groups.find((x) => x.groupname === suggestion.groupname);

        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var tmp = participants[index];

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        tmp.groupId = group.groupId;

        participants[index] = tmp;

        this.setState({
            participants: participants
        });

        return suggestion.groupname;
    }


    renderSuggestion(suggestion) {
        return (
            <span>{suggestion.groupname}</span>
        );
    }


    onChange(participantId, proxy, { newValue, method }) {
        var groups = this.state.groups;
        var group = groups.find((x) => x.groupname === newValue);

        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var tmp = participants[index];

        tmp.groupName = newValue;

        if (group) {
            tmp.groupId = group.groupId;
        } else {
            tmp.groupId = 0;
        }

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        participants[index] = tmp;

        this.setState({
            participants: participants
        });
    };


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderParticipantsTable(this.state.participants);

        return (
            <div>
                <h1>Race Participants</h1>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" onClick={this.handleAddParticipant} disabled={this.dirty} className="btn btn-primary">Add Participant</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
