import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import Autosuggest from 'react-autosuggest';
import 'react-toastify/dist/ReactToastify.css';


export class Participants extends Component {
    static displayName = Participants.name;

    newParticipantsCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleAddParticipant = this.handleAddParticipant.bind(this);
        this.handleBlur = this.handleBlur.bind(this);

        this.onSuggestionsFetchRequested = this.onSuggestionsFetchRequested.bind(this);
        this.onSuggestionsClearRequested = this.onSuggestionsClearRequested.bind(this);
        this.onChange = this.onChange.bind(this);
        this.getSuggestionValue = this.getSuggestionValue.bind(this);
        this.getInputProps = this.getInputProps.bind(this);


        this.state = { participants: [], categories: [], suggestions: [], value: "", loading: true };

        fetch('api/Participant/GetAllAvailableParticipants')
            .then(response => response.json())
            .then(data => {
                this.setState({ participants: data, activePage: 1 });
                fetch('api/Category/GetAvailableCategories')
                    .then(response => response.json())
                    .then(data => {
                        this.setState({ categories: data, suggestions : data, loading: false, activePage: 1 });
                    });
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


    handleBlur(participantId) {
        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var participant = participants[index];

        if (participant.toAdd) {
            this.addParticipant(participant);
        } else if (participant.toUpdate) {
            this.updateParticipant(participant);
        } else if (participant.toDelete) {

        } else {
            // TODO
        }
    }


    handleAddParticipant() {
        var newParticipant = {
            participantId: this.newParticipantsCounter,
            firstname: "",
            lastname: "",
            category: "",
            yearOfBirth: "",
            toAdd: true,
        }

        this.newParticipantsCounter--;

        var participants = this.state.participants;
        participants.push(newParticipant);

        this.setState({
            participants: participants
        });
    }


    addParticipant(participant) {
        fetch('api/Participant/AddParticipant', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(participant)
        })
            .then(response => response.json())
            .then(data => {
                var participants = this.state.participants;
                var index = participants.findIndex((x) => x.participantId === participant.participantId);
                var oldParticipant = participants[index];
                oldParticipant = data;
                participants[index] = oldParticipant;

                this.setState({
                    participants: participants
                });

                toast("Participant: " + participant.firstname + " " + participant.lastname + " added successfully");
            });
    }


    updateParticipant(participant) {
        fetch('api/Participant/UpdateParticipant', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(participant)
        })
            .then(response => response.json())
            .then(data => {
                var participants = this.state.participants;
                var index = participants.findIndex((x) => x.participantId === data.participantId);
                var oldParticipant = participants[index];
                oldParticipant = data;
                participants[index] = oldParticipant;

                this.setState({
                    participants: participants
                });

                toast("Participant: " + participant.firstname + " " + participant.lastname + " updated successfully");
            });
    }


    getInputProps(category, participantId) {
        var categoryName = "";

        if (!category) {
            categoryName = "";
        } else {
            categoryName = category;
        }

        return {
            placeholder: "Lauf/Rad",
            value: categoryName,
            onChange: this.onChange.bind(this, participantId),
            onBlur: this.handleBlur.bind(this, participantId)
        };
    }


    getSuggestions = value => {
        const inputValue = value.trim().toLowerCase();
        const inputLength = inputValue.length;

        var result = inputLength === 0 
            ? [] 
            : this.state.categories.filter(category =>
                category.categoryName.toLowerCase().slice(0, inputLength) === inputValue
        );

        return result;
    };


    onSuggestionsFetchRequested = ({ value }) => {
        this.setState({
            suggestions: this.getSuggestions(value)
        });
    };


    onSuggestionsClearRequested = () => {
        this.setState({
            suggestions: this.state.categories,
        });
    };


    getSuggestionValue(participantId, suggestion) {
        var categories = this.state.categories;
        var category = categories.find((x) => x.categoryName === suggestion.categoryName);

        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var tmp = participants[index];

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        tmp.category = category.groupId;

        participants[index] = tmp;

        this.setState({
            participants: participants
        });

        return suggestion.categoryName;
    }


    renderSuggestion(suggestion) {
        return (
            <span>{suggestion.categoryName}</span>
        );
    }


    onChange(participantId, proxy, { newValue, method }) {
        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var tmp = participants[index];

        tmp.category = newValue;

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        participants[index] = tmp;

        this.setState({
            participants: participants
        });
    };


    renderParticipantsTable(participants) {
        return (
            <div>
                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Firstname</th>
                            <th>Lastname</th>
                            <th>Category</th>
                            <th>Year Of Birth</th>
                        </tr>
                    </thead>
                    <tbody>
                        {participants.map(participant =>
                            <tr key={participant.participantId}>
                                <td>
                                    <input autoFocus="false" type="text" id="Firstname" onChange={this.handleChange.bind(this, participant.participantId)} onBlur={this.handleBlur.bind(this, participant.participantId)} value={participant.firstname}></input>
                                </td>
                                <td>
                                    <input type="text" id="Lastname" onChange={this.handleChange.bind(this, participant.participantId)} onBlur={this.handleBlur.bind(this, participant.participantId)} value={participant.lastname}></input>
                                </td>
                                <td>
                                    <Autosuggest
                                        suggestions={this.state.suggestions}
                                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                                        getSuggestionValue={this.getSuggestionValue.bind(this, participant.participantId)}
                                        renderSuggestion={this.renderSuggestion}
                                        inputProps={this.getInputProps(participant.category, participant.participantId)}
                                    />
                                </td>
                                <td>
                                    <input type="text" id="YearOfBirth" onChange={this.handleChange.bind(this, participant.participantId)} onBlur={this.handleBlur.bind(this, participant.participantId)} value={participant.yearOfBirth}></input>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderParticipantsTable(this.state.participants);

        return (
            <div>
                <h1>Participants</h1>
                <form>
                    <div>
                        <button type="button" onClick={this.handleAddParticipant} disabled={this.dirty} className="btn btn-primary">Add Participant</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
