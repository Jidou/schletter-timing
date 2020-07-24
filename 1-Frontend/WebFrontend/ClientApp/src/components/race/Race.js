import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { SortableContainer, SortableElement } from 'react-sortable-hoc';
import arrayMove from 'array-move';
import Autosuggest from 'react-autosuggest';


const SortableItem = SortableElement(({ value, removeHandler, changeHandler }) =>
    <div className="input-group mb-3">
        <div className="input-group-prepend">
            <span className="input-group-text">=</span>
            <input type="text" className="form-control" placeholder="Groupname" aria-label="Username" aria-describedby="addon-wrapping" onChange={changeHandler.bind(this, value.groupId, value.startNumber)} value={value.startNumber} />
        </div>
        <input type="text" className="form-control" placeholder="Groupname" aria-label="Username" aria-describedby="addon-wrapping" readOnly value={value.groupname} />
        <div className="input-group-append">
            <button className="btn btn-danger" onClick={removeHandler.bind(this, value.groupname)} type="button">Remove</button>
        </div>
    </div>);


const SortableList = SortableContainer(({ items, removeHandler, changeHandler }) => {
    return (
        <ul>
            {items.map((item) => (
                <SortableItem key={`item-${item.groupId}`} index={item.startNumber} value={item} removeHandler={removeHandler} changeHandler={changeHandler} />
            ))}
        </ul>
    );
});


class SortableComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            items: props.items,
        };
    }


    componentDidMount() {
        if (this.props.items) {
            this.setState({
                items: this.props.items
            });
        }
    }


    onSortEnd = ({ oldIndex, newIndex }) => {
        var tmpItems = this.state.items;


        var tmp1 = tmpItems[oldIndex-1];
        var tmp2 = tmpItems[newIndex-1];

        tmpItems[oldIndex-1] = tmp2;
        tmpItems[newIndex-1] = tmp1;

        this.setState({
            items: tmpItems
        });

        //this.setState(({ items }) => ({
        //    items: arrayMove(items, oldIndex, newIndex),
        //}));
    };

    render() {
        return <SortableList items={this.state.items} changeHandler={this.props.changeHandler} removeHandler={this.props.removeHandler} onSortEnd={this.onSortEnd} />;
    }
}


export class Race extends Component {
    static displayName = Race.name;

    dirty = false;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChangeInForm.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleShuffle = this.handleShuffle.bind(this);
        this.handleAddGroup = this.handleAddGroup.bind(this);
        this.onChange = this.onChange.bind(this);
        this.handleRemoveFromChild = this.handleRemoveFromChild.bind(this);
        this.handleChangeInChild = this.handleChangeInChild.bind(this);
        this.handleChangeInTable = this.handleChangeInTable.bind(this);
        this.handleTableBlur = this.handleTableBlur.bind(this);
        this.state = { race: [], groups: [], allgroups: [], suggestions: [], searchValue: "", loading: true };

        if (this.props.match.path === '/race/loadrace/:name') {
            var racename = this.props.match.params.name;

            fetch('api/Race/LoadRace?racename=' + racename)
                .then(response => response.json())
                .then(data => {
                    this.setState({ race: data });
                });

            fetch('api/Race/GetGroupInfoForRace?racename=' + racename)
                .then(response => response.json())
                .then(data => {
                    this.setState({ groups: data });
                });

        } else if (this.props.match.url === '/race/newrace/') {
            fetch('api/Race/CreateNewRace', {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify()
            })
                .then(response => response.json())
                .then(data => {
                    this.setState({ race: data });
                });

        } else if (this.props.match.url === '/race/race/') {
            fetch('api/Race')
                .then(response => response.json())
                .then(data => {
                    this.setState({ race: data });
                });

            fetch('api/RaceGroup/GetGroupInfoForRace')
                .then(response => response.json())
                .then(data => {
                    this.setState({ groups: data });
                });
        } else {
            // TODO
        }

        fetch('api/Group/')
            .then(response => response.json())
            .then(data => {
                this.setState({ allgroups: data, suggestions: data, loading: false });
            });

    }


    renderRaceForm() {
        return (
            <div>
                <div>
                    <div className="form-group">
                        <label htmlFor="Titel">Racename</label>
                        <input type="text" className="form-control" id="Titel" onChange={this.handleChangeInForm} onBlur={this.handleBlur} value={this.state.race.titel} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="RaceType">RaceType</label>
                        <input type="text" className="form-control" id="RaceType" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="Some name" value={this.state.race.raceType} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Place">Place</label>
                        <input type="text" className="form-control" id="Place" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="Some name" value={this.state.race.place} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Date">Date</label>
                        <input type="date" className="form-control" id="Date" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="2019-05-26" value={this.state.race.date} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Judge">Judge</label>
                        <input type="text" className="form-control" id="Judge" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="Some name" value={this.state.race.judge} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="TimingTool">TimingTool</label>
                        <input type="text" className="form-control" id="TimingTool" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="AlgeTiming" value={this.state.race.timingTool} />
                    </div>
                    {/* <button type="submit" className="btn btn-primary">Save</button> */}
                    <button type="button" onClick={this.handleShuffle} disabled={this.dirty} className="btn btn-primary">Assign Startnumbers</button>
                </div>

                {/* <div className="form-group">
                    <Autosuggest
                        suggestions={this.state.suggestions}
                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                        getSuggestionValue={this.getSuggestionValue}
                        renderSuggestion={this.renderSuggestion}
                        inputProps={this.getInputProps()}
                    />
                    <button type="button" onClick={this.handleAddGroup} disabled={this.dirty} className="btn btn-primary">Add Group</button>
                </div> */}

                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Start Number</th>
                            <th>Group Name</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.groups.map(group =>
                            <tr key={group.groupId}>
                                <td>
                                    <input type="text" id="groupname" onChange={this.handleChangeInTable.bind(this, group.groupId)} onBlur={this.handleTableBlur.bind(this, group.groupId)} value={group.startNumber}></input>
                                </td>
                                <td>{group.groupname}</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
                {/* <SortableComponent items={this.state.race.groups} removeHandler={this.handleRemoveFromChild} changeHandler={this.handleChangeInChild} /> */}
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


    handleChangeInChild(groupId, startNumber) {
        console.log("safasfsdfasdffads2121212");

        var race = this.state.race;
        var groups = race.groups;
        var index = groups.findIndex((x) => x.groupId === groupId);

        var group1 = groups.find((x) => x.groupId === groupId);

        group1.startNumber = startNumber;
        groups[index] = groups;
        race.groups = groups;

        this.setState({
            race: race
        })
    }


    handleRemoveFromChild(value) {
        console.log("safasfsdfasdffads");
        var race = this.state.race;

        for (var i = 0; i < race.groups.length; i++) {
            if (race.groups[i].groupname === value) {
                race.groups.splice(i, 1);
            }
        }

        this.setState({
            race: race
        });
    }


    handleChangeInForm(event) {
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


    handleBlur(groupId, event) {

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


    handleShuffle() {
        fetch('api/Race/AssignStartNumbers/')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data, loading: false });
            });
    }


    handleChangeInTable(groupId, event) {
        
    }


    handleTableBlur(groupId, event) {

    }


    handleAddGroup() {
        var race = this.state.race;
        var newGroup = this.state.groups.find((x) => x.groupname === this.state.searchValue);

        if (!newGroup) {
            return;
        }

        var groups = this.state.groups;

        var maxStartNumber = Math.max.apply(Math, groups.map(function (o) { return o.startNumber; }))
        newGroup.startNumber = ++maxStartNumber;

        race.groups.push(newGroup);

        this.setState({
            race: race
        });
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderRaceForm();

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
