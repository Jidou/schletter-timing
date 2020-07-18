import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import Autosuggest from 'react-autosuggest';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Groups extends Component {
    static displayName = Groups.name;

    newGroupsCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleAddGroup = this.handleAddGroup.bind(this);
        this.onChange = this.onChange.bind(this);
        this.updateGroupParticipants = this.updateGroupParticipants.bind(this);
        this.updateGroup = this.updateGroup.bind(this);

        this.state = { groups: [], allParticipants: [], suggestions: [], searchValue: "", loading: true };

        fetch('api/Group/')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data });
            });

        fetch('api/Participant/GetAllParticipantsWithoutGroup')
            .then(response => response.json())
            .then(data => {
                this.setState({ allParticipants: data, suggestions: data, loading: false, activePage: 1 });
            });
    }


    handleChange(groupId, event) {
        var groups = this.state.groups;
        var index = groups.findIndex((x) => x.groupId === groupId);
        var target = event.target.id;
        var value = event.target.value;
        var tmp = groups[index];

        if (target === "groupname") {
            tmp.groupname = value;
        } else if (target === "groupclass") {
            tmp.class = value;
        } else {
            return;
        }

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        groups[index] = tmp;

        this.setState({
            groups: groups
        });
    }


    handleBlur(groupId, event) {
        var groups = this.state.groups;
        var index = groups.findIndex((x) => x.groupId === groupId);
        var group = groups[index];

        if (group.toAdd) {
            this.addGroup(group);
        } else if (group.toUpdate) {
            this.updateGroup(group);
        } else if (group.toDelete) {

        } else {
            // TODO
        }
    }


    handleAddGroup() {
        var newGroup = {
            groupId: this.newGroupsCounter,
            groupname: "",
            class: "",
            participant1Id: 0,
            participant1FullName: "",
            participant2Id: 0,
            participant2FullName: "",
            toAdd: true,
        }

        this.newGroupsCounter--;

        var groups = this.state.groups;
        groups.push(newGroup);

        this.setState({
            groups: groups
        });
    }


    handleSubmit(event) {
        fetch('api/Group/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.groups)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data });
            });


        event.preventDefault();
        toast("Groups saved successfully");
    }


    showGroupDetails() {

    }


    renderGroupsTable(groups) {
        return (
            <div>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>Groupname</th>
                            <th>Class</th>
                            <th>Participant 1</th>
                            <th>Participant 2</th>
                        </tr>
                    </thead>
                    <tbody>
                        {groups.map(group =>
                            <tr key={group.groupId}>
                                <td>
                                    <input type="text" id="groupname" onChange={this.handleChange.bind(this, group.groupId)} onBlur={this.handleBlur.bind(this, group.groupId)} value={group.groupname}></input>
                                </td>
                                <td>
                                    <input type="text" id="groupclass" onChange={this.handleChange.bind(this, group.groupId)} onBlur={this.handleBlur.bind(this, group.groupId)} value={group.class}></input>
                                </td>
                                <td>
                                    <Autosuggest id={group.groupId + "1"}
                                        suggestions={this.state.suggestions}
                                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                                        getSuggestionValue={this.getSuggestionValue.bind(this, group.groupId, 1)}
                                        renderSuggestion={this.renderSuggestion}
                                        inputProps={this.getInputProps(group.groupId, group.participant1Id, group.participant1FullName, 1)}
                                    />
                                </td>
                                <td>
                                    <Autosuggest id={group.groupId + "2"}
                                        suggestions={this.state.suggestions}
                                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                                        getSuggestionValue={this.getSuggestionValue.bind(this, group.groupId, 2)}
                                        renderSuggestion={this.renderSuggestion}
                                        inputProps={this.getInputProps(group.groupId, group.participant2Id, group.participant2FullName, 2)}
                                    />
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>

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


    getInputProps(groupId, participantId, participantfullname, participantNumber) {
        if (participantfullname === null) {
            participantfullname = "";
        }

        return {
            placeholder: "Name",
            value: participantfullname,
            onChange: this.onChange.bind(this, groupId, participantId, participantfullname, participantNumber)
        };
    }


    getSuggestions = value => {
        const inputValue = value.trim().toLowerCase();
        const inputLength = inputValue.length;

        return inputLength === 0 ? [] : this.state.allParticipants.filter(participant =>
            participant.fullname.toLowerCase().slice(0, inputLength) === inputValue
        );
    };


    onSuggestionsFetchRequested = ({ value }) => {
        this.setState({
            suggestions: this.getSuggestions(value)
        });
    };


    onSuggestionsClearRequested = () => {
        this.setState({
            suggestions: this.state.allParticipants,
        });
    };


    getSuggestionValue(groupId, participantNumber, suggestion) {
        var allParticipants = this.state.allParticipants;
        var participant = allParticipants.find((x) => x.fullname === suggestion.fullname);

        var groups = this.state.groups;
        var index = groups.findIndex((x) => x.groupId === groupId);
        var group = groups[index];

        if (participantNumber === 1) {
            group.participant1Id = participant.participantId;
            group.participant1FullName = participant.fullname;
        } else if (participantNumber === 2) {
            group.participant2Id = participant.participantId;
            group.participant2FullName = participant.fullname;
        } else {
            //TODO
        }

        groups[index] = group;

        this.setState({
            groups: groups
        });

        this.updateGroupParticipants(group);

        return suggestion.fullname;
    }


    renderSuggestion(suggestion) {
        return (
            <span>{suggestion.fullname}</span>
        );
    }


    onChange(groupId, participantId, participantfullname, participantNumber, proxy, { newValue, method }) {
        if (method == "click" || newValue === null) {
            return;
        }

        var groupNeedsUpdate = false;

        var groups = this.state.groups;
        var index = groups.findIndex((x) => x.groupId === groupId);
        var group = groups[index];

        if (participantNumber === 1) {
            if (group.participant1Id !== 0 && group.participant1FullName !== newValue) {
                group.participant1Id = 0;
                groupNeedsUpdate = true;
            }
            group.participant1FullName = newValue;
        } else if (participantNumber === 2) {
            if (group.participant2Id !== 0 && group.participant2FullName !== newValue) {
                group.participant2Id = 0;
                groupNeedsUpdate = true;
            }
            group.participant2FullName = newValue;
        } else {
            // TODO
        }

        if (groupNeedsUpdate) {
            this.updateGroupParticipants(group);
        }

        groups[index] = group;

        this.setState({
            searchValue: newValue
        });
    };


    addGroup(group) {
        fetch('api/Group/AddGroup', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(group)
        })
            .then(response => response.json())
            .then(data => {
                var groups = this.state.groups;
                var index = groups.findIndex((x) => x.groupId === group.groupId);
                var oldGroup = groups[index];
                oldGroup = data;
                groups[index] = oldGroup;

                this.setState({
                    groups: groups
                });

                toast("Group: " + group.groupname + " successfully added");
            })
    }


    updateGroup(group) {
        fetch('api/Group/UpdateGroup', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(group)
        })
            .then(response => response.json())
            .then(data => {
                var groups = this.state.groups;
                var index = groups.findIndex((x) => x.groupId === data.groupId);
                var oldGroup = groups[index];
                oldGroup = data;
                groups[index] = oldGroup;

                this.setState({
                    groups: groups
                });

                toast("Group: " + group.groupname + " successfully updated");
            })
    }


    updateGroupParticipants(group) {
        fetch('api/Group/UpdateGroupParticipants', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(group)
        })
            .then(response => response.json())
            .then(data => {
                var groups = this.state.groups;
                var index = groups.findIndex((x) => x.groupId === data.groupId);
                var group = groups[index];
                groups[index] = group;

                this.setState({
                    groups: groups
                });

                toast("Participants of Group: " + group.groupname + " successfully updated");
            })
            .then(fetch('api/Participant/GetAllParticipantsWithoutGroup')
                .then(response => response.json())
                .then(data => {
                    this.setState({ allParticipants: data, suggestions: data });
                })
            );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderGroupsTable(this.state.groups);

        return (
            <div>
                <h1>Groups</h1>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" onClick={this.handleAddGroup} disabled={this.dirty} className="btn btn-primary">Add Group</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
