import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import Autosuggest from 'react-autosuggest';

import 'react-toastify/dist/ReactToastify.css';


export class RaceGroups extends Component {
    static displayName = RaceGroups.name;

    newGroupsCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.onChange = this.onChange.bind(this);
        this.handleAddGroup = this.handleAddGroup.bind(this);

        this.state = { groups: [], allgroups: [], suggestions: [], searchValue: "", loading: true };

        fetch('api/RaceGroup/GetAllGroupsOfRace')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data});
            });

        fetch('api/Group/GetAllAvailableGroups')
            .then(response => response.json())
            .then(data => {
                this.setState({ allgroups: data, suggestions: data, loading: false, activePage: 1  });
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

        this.updateGroup(group);
    }


    updateGroup(group) {
        fetch('api/RaceGroup/UpdateGroupToRace', {
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


    handleAddGroup() {
        var newGroup = this.state.allgroups.find((x) => x.groupname === this.state.searchValue);

        if (!newGroup) {
            toast(this.state.searchValue + " is not a valid group");
            return;
        }

        var groups = this.state.groups;

        var maxStartNumber = Math.max.apply(Math, groups.map(function (o) { return o.startNumber; }))
        newGroup.startNumber = ++maxStartNumber;
        newGroup.toAdd = true;

        groups.push(newGroup);

        this.setState({
            groups: groups
        });

        fetch('api/RaceGroup/AddGroupToRace', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newGroup)
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

                toast("Group successfully added");
            });
    }


    renderGroupsTable(groups) {
        return (
            <div>
                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Startnummer</th>
                            <th>Groupname</th>
                            <th>Class</th>
                            <th>Läufer</th>
                            <th>Rad/E-Bike</th>
                        </tr>
                    </thead>
                    <tbody>
                        {groups.map(group =>
                            <tr key={group.groupId}>
                                <td>{group.startNumber}</td>
                                <td>
                                    <input type="text" id="groupname" onChange={this.handleChange.bind(this, group.groupId)} onBlur={this.handleBlur.bind(this, group.groupId)} value={group.groupname}></input>
                                </td>
                                <td>
                                    <input type="text" id="groupclass" onChange={this.handleChange.bind(this, group.groupId)} onBlur={this.handleBlur.bind(this, group.groupId)} value={group.class}></input>
                                </td>
                                <td>{group.participant1FullName}</td>
                                <td>{group.participant2FullName}</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }


    getInputProps() {
        return {
            placeholder: "Groupname",
            value: this.state.searchValue,
            onChange: this.onChange
        };
    }


    getSuggestions = value => {
        const inputValue = value.trim().toLowerCase();
        const inputLength = inputValue.length;

        return inputLength === 0 ? [] : this.state.allgroups.filter(group =>
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
            suggestions: this.state.allgroups,
        });
    };


    getSuggestionValue(suggestion) {
        return suggestion.groupname;
    }


    renderSuggestion(suggestion) {
        return (
            <span>{suggestion.groupname}</span>
        );
    }


    onChange(proxy, { newValue }) {
        this.setState({
            searchValue: newValue
        });
    };


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderGroupsTable(this.state.groups);

        return (
            <div>
                <h1>Race Groups</h1>
                <form>
                    <div className="form-group">
                        <Autosuggest
                            suggestions={this.state.suggestions}
                            onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                            onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                            getSuggestionValue={this.getSuggestionValue}
                            renderSuggestion={this.renderSuggestion}
                            inputProps={this.getInputProps()}
                        />
                        <button type="button" onClick={this.handleAddGroup} className="btn btn-primary">Add Group</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
