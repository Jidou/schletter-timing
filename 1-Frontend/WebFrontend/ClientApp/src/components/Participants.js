import React, { Component } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Participants extends Component {
    static displayName = Participants.name;

    newParticipantsCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleAddParticipant = this.handleAddParticipant.bind(this);
        this.onChange = this.onChange.bind(this);
        this.state = { participants: [], suggestions: [], value: "", loading: true };

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


    onChange(participantId, proxy, { newValue, method }) {
        var participants = this.state.participants;
        var index = participants.findIndex((x) => x.participantId === participantId);
        var tmp = participants[index];

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
                <h1>Participants</h1>
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
