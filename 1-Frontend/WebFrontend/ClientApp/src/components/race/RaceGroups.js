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

        this.state = { groups: [], searchValue: "", loading: true };

        fetch('api/RaceGroup/GetAllGroupsOfRace')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data, loading: false, activePage: 1});
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
        } else if (target === "participant1") {
            tmp.participant1FullName = value;
        } else if (target === "participant2") {
            tmp.participant2FullName = value;
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

        this.updateGroups(groups);
    }


    updateGroups(groups) {
        fetch('api/RaceGroup/UpdateGroups', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(groups)
        })
            .then(response => response.json())
            .then(data => {
                var groups = this.state.groups;

                this.setState({
                    groups: groups
                });

                toast("Groups: successfully updated");
            })
    }


    handleAddGroup() {
        var groups = this.state.groups;

        var maxGroupId = Math.max.apply(Math, groups.map(function (o) { return o.groupId; }))
        var maxStartNumber = Math.max.apply(Math, groups.map(function (o) { return o.startNumber; }))

        var newGroup = {
            groupname: "",
            groupId: ++maxGroupId,
            startNumber: maxStartNumber === 0 ? 0 : ++maxStartNumber,
            toAdd: true

        }

        groups.push(newGroup);

        this.setState({
            groups: groups
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
                                <td>
                                    <input type="text" id="participant1" onChange={this.handleChange.bind(this, group.groupId)} onBlur={this.handleBlur.bind(this, group.groupId)} value={group.participant1FullName}></input>
                                </td>
                                <td>
                                    <input type="text" id="participant2" onChange={this.handleChange.bind(this, group.groupId)} onBlur={this.handleBlur.bind(this, group.groupId)} value={group.participant2FullName}></input>
                                </td>
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
                        <button type="button" onClick={this.handleAddGroup} className="btn btn-primary">Add Group</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
