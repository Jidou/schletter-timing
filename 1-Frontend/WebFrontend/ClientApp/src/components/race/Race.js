import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { SortableContainer, SortableElement } from 'react-sortable-hoc';
import arrayMove from 'array-move';
import Autosuggest from 'react-autosuggest';
import { read } from 'convert-csv-to-json';

let csvToJson = require('convert-csv-to-json');
// let fs = require('fs-js');


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


    constructor(props) {
        super(props);
        this.handleChangeInForm = this.handleChangeInForm.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.handleShuffle = this.handleShuffle.bind(this);
        this.handleAddGroup = this.handleAddGroup.bind(this);
        this.handleRemoveFromChild = this.handleRemoveFromChild.bind(this);
        this.handleChangeInChild = this.handleChangeInChild.bind(this);
        this.handleChangeInTable = this.handleChangeInTable.bind(this);
        this.handleTableBlur = this.handleTableBlur.bind(this);
        this.handleUpload = this.handleUpload.bind(this);
        this.onFileChange = this.onFileChange.bind(this);
        // this.state = { race: [], groups: [], suggestions: [], searchValue: "", newRace: false, selectedFile: null, loading: true };
        this.state = { race: [], newRace: false, selectedFile: null, loading: true };

        if (this.props.match.url === '/race/newrace') {
            fetch('api/Race/CreateNewRace')
                .then(response => response.json())
                .then(data => {
                    this.setState({ race: data, newRace: true, loading: false });
                });
        } else {
            fetch('api/Race/LoadRace')
                .then(response => response.json())
                .then(data => {
                    this.setState({ race: data, newRace: false, loading: false });
                });
        }
    }


    renderRaceFormAndTable() {
        return (
            <div>
                { this.renderRaceForm() }
                { this.state.race.groups.length > 0 ? this.assignNumbersButton() : null }
                { this.state.newRace ? this.importButton() : null }
                {/* { !this.state.newRace ? this.groupMenu() : null} */}
                { this.state.race.groups.length > 0 ? this.renderTable() : null }
                {/* <SortableComponent items={this.state.race.groups} removeHandler={this.handleRemoveFromChild} changeHandler={this.handleChangeInChild} /> */}
            </div>
        );
    }


    renderRaceForm = () => (
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
                <input type="text" className="form-control" id="Date" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="2019-05-26" value={this.state.race.date} />
            </div>
            <div className="form-group">
                <label htmlFor="Judge">Judge</label>
                <input type="text" className="form-control" id="Judge" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="Some name" value={this.state.race.judge} />
            </div>
            <div className="form-group">
                <label htmlFor="TimingTool">TimingTool</label>
                <input type="text" className="form-control" id="TimingTool" onChange={this.handleChangeInForm} onBlur={this.handleBlur} placeholder="AlgeTiming" value={this.state.race.timingTool} />
            </div>
        </div>
    )


    assignNumbersButton = () => (
        <div>
            <button type="button" onClick={this.handleShuffle} className="btn btn-primary">Assign Startnumbers</button>
        </div>
    )


    importButton = () => (
        <div>
            <div>
                <input type="file" accept=".csv,.xlsx,.xls" onChange={this.onFileChange} className="form-control form-control-lg"/>
            </div>
            <div>
                <button type="button" onClick={this.handleUpload} className="btn btn-primary">Upload</button>
            </div>
        </div>
    )

    groupMenu = () => (
        {/* <div className="form-group">
                    <Autosuggest
                        suggestions={this.state.suggestions}
                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                        getSuggestionValue={this.getSuggestionValue}
                        renderSuggestion={this.renderSuggestion}
                        inputProps={this.getInputProps()}
                    />
                    <button type="button" onClick={this.handleAddGroup} className="btn btn-primary">Add Group</button>
                </div> */}
    )

    renderTable = () => (
        <Table striped hover>
            <thead>
                <tr>
                    <th>Start Number</th>
                    <th>Group Name</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                {this.state.race.groups.map(group =>
                    <tr key={group.groupId}>
                        <td>
                            <input type="text" id="groupname" onChange={this.handleChangeInTable.bind(this, group.groupId)} onBlur={this.handleTableBlur.bind(this, group.groupId)} value={group.startNumber}></input>
                        </td>
                        <td>{group.groupname}</td>
                    </tr>
                )}
            </tbody>
        </Table>
    )


    onFileChange(event) {
        this.setState({ selectedFile: event.target.files[0] });

    }


    handleUpload() {
        const reader = new FileReader()
        reader.onload = async (event) => {
            const text = (event.target.result);

            var fileAsJson = csvToJson.csvStringToJson(text);

            for (let i = 0; i < fileAsJson.length; i++) {
                console.log(fileAsJson[i]);
            }

            fetch('api/Race/SetCurrentRace?racename=' + this.state.race.titel)
                .then(_ => {
                    fetch('api/Race/UpdateRace', {
                        method: 'POST',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(this.state.race)
                    })
                    .then(_ => {
                        fetch('api/Upload/Upload', {
                            method: 'POST',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                            },
                            body: JSON.stringify(fileAsJson)
                        })
                        .then(response => response.json())
                        .then(data => {
                            this.setState({ race: data, newRace: false, loading: false });
                        });

                    })
                        .then(toast("Race saved successfully"));
                });


        };
        reader.readAsText(this.state.selectedFile)
    }


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
    }


    handleBlur(event) {
        if (this.state.newRace) {
            return;
        }

        if (this.state.newRace && this.state.race.titel && this.state.race.titel !== "") {
            fetch('api/Race/SetCurrentRace?racename=' + this.state.race.titel)
                .then(_ => {
                    // this.setState({ newRace: false });

                    fetch('api/Race/UpdateRace', {
                        method: 'POST',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(this.state.race)
                    })
                    .then(toast("Race saved successfully"));
                });
        } else {
            fetch('api/Race/UpdateRace', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(this.state.race)
            })
            .then(toast("Race saved successfully"));
        }
    }


    handleShuffle() {
        fetch('api/Race/AssignStartNumbers', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.race)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data, loading: false });
                toast("Startnumbers have been assigned");
            });
    }


    handleChangeInTable(groupId, event) {
        
    }


    handleTableBlur(groupId, event) {

    }


    handleAddGroup() {
        var race = this.state.race;
        var newGroup = this.state.race.groups.find((x) => x.groupname === this.state.searchValue);

        if (!newGroup) {
            return;
        }

        var groups = this.state.race.groups;

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
            : this.renderRaceFormAndTable();

        return (
            <div>
                <h1>Race</h1>
                <form>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
