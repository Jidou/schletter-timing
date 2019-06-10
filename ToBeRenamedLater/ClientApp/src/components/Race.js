import React, { Component } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { SortableContainer, SortableElement } from 'react-sortable-hoc';
import arrayMove from 'array-move';
import Autosuggest from 'react-autosuggest';


const SortableItem = SortableElement(({ value, removeHandler }) =>
    <div className="input-group mb-3">
        <div className="input-group-prepend">
            <span className="input-group-text">=</span>
        </div>
        <input type="text" className="form-control" placeholder="Groupname" aria-label="Username" aria-describedby="addon-wrapping" readOnly value={value} />
        <div className="input-group-append">
            <button className="btn btn-danger" onClick={removeHandler.bind(this, value)} type="button">Remove</button>
        </div>
    </div>);


const SortableList = SortableContainer(({ items, removeHandler }) => {
    return (
        <ul>
            {items.map((item) => (
                <SortableItem key={`item-${item.groupId}`} index={item.groupId} value={item.groupname} removeHandler={removeHandler}/>
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
        this.setState(({ items }) => ({
            items: arrayMove(items, oldIndex, newIndex),
        }));
    };

    render() {
        return <SortableList items={this.state.items} removeHandler={this.props.removeHandler} onSortEnd={this.onSortEnd} />;
    }
}


export class Race extends Component {
    static displayName = Race.name;

    dirty = false;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleLoad = this.handleLoad.bind(this);
        this.handleAddGroup = this.handleAddGroup.bind(this);
        this.onChange = this.onChange.bind(this);
        this.handleRemoveFromChild = this.handleRemoveFromChild.bind(this);
        this.state = { race: [], groups: [], suggestions: [], searchValue: "", loading: true };

        fetch('api/Race/')
            .then(response => response.json())
            .then(data => {
                this.setState({ race: data, loading: false });
            });

        fetch('api/Group/GetIdAndNameOnly')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data, suggestions: data });
            });
    }


    renderRaceForm() {
        return (
            <div>
                <div>
                    <div className="form-group">
                        <label htmlFor="Titel">Racename</label>
                        <input type="text" className="form-control" id="Titel" onChange={this.handleChange} value={this.state.race.titel}></input>
                    </div>
                    <div className="form-group">
                        <label htmlFor="RaceType">RaceType</label>
                        <input type="text" className="form-control" id="RaceType" onChange={this.handleChange} placeholder="Some name" value={this.state.race.raceType} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Place">Place</label>
                        <input type="text" className="form-control" id="Place" onChange={this.handleChange} placeholder="Some name" value={this.state.race.place} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Date">Date</label>
                        <input type="date" className="form-control" id="Date" onChange={this.handleChange} placeholder="2019-05-26" value={this.state.race.date} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Judge">Judge</label>
                        <input type="text" className="form-control" id="Judge" onChange={this.handleChange} placeholder="Some name" value={this.state.race.judge} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="TimingTool">TimingTool</label>
                        <input type="text" className="form-control" id="TimingTool" onChange={this.handleChange} placeholder="AlgeTiming" value={this.state.race.timingTool} />
                    </div>
                    <button type="submit" className="btn btn-primary">Save</button>
                    <button type="button" onClick={this.handleLoad} disabled={this.dirty} className="btn btn-primary">Load</button>
                </div>

                <div className="form-group">
                    <Autosuggest
                        suggestions={this.state.suggestions}
                        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                        getSuggestionValue={this.getSuggestionValue}
                        renderSuggestion={this.renderSuggestion}
                        inputProps={this.getInputProps()}
                    />
                    <button type="button" onClick={this.handleAddGroup} disabled={this.dirty} className="btn btn-primary">Add Group</button>
                </div>

                <SortableComponent items={this.state.race.groups} removeHandler={this.handleRemoveFromChild}/>
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


    handleAddGroup() {
        var race = this.state.race;
        var newGroup = this.state.groups.find((x) => x.groupname === this.state.searchValue);
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
