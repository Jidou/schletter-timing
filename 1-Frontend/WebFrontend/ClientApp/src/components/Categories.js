import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Categories extends Component {
    static displayName = Categories.name;

    newCategoriesCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleAdd = this.handleAdd.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.state = { categories: [], loading: true };

        fetch('api/Category/GetAvailableCategories')
            .then(response => response.json())
            .then(data => {
                this.setState({ categories: data, loading: false });
            });
    }


    handleChange(categoryId, event) {
        var categories = this.state.categories;
        var index = categories.findIndex((x) => x.categoryId === categoryId);
        var value = event.target.value;
        var tmp = categories[index];

        tmp.categoryName = value;
        categories[index] = tmp;

        this.setState({categories: categories});
    }


    handleBlur(categoryId) {
        var categories = this.state.categories;
        var index = categories.findIndex((x) => x.categoryId === categoryId);
        var category = categories[index];

        if (category.categoryId > 0) {
            this.updateCategory(category);
        } else {
            this.addCategory(category);
        }
    }


    handleAdd() {
        var newCategory = {
            categoryId: this.newCategoriesCounter,
            categoryName: "",
        }

        this.newCategoriesCounter--;

        var categories = this.state.categories;
        categories.push(newCategory);

        this.setState({categories: categories});
    }


    addCategory(category) {
        fetch('api/Category/AddCategory', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(category)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({categories: data});
                toast("Category: " + category.categoryName + " added successfully");
            });
    }


    updateCategory(category) {
        fetch('api/Category/UpdateCategory', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(category)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({categories: data});
                toast("category: " + category.categoryName + " updated successfully");
            });
    }


    renderTable(categories) {
        return (
            <div>
                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Category</th>
                        </tr>
                    </thead>
                    <tbody>
                        {categories.map(category =>
                            <tr key={category.categoryId}>
                                <td>
                                    <input type="text" onChange={this.handleChange.bind(this, category.categoryId)} onBlur={this.handleBlur.bind(this, category.categoryId)} value={category.categoryName}></input>
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
            : this.renderTable(this.state.categories);

        return (
            <div>
                <h1>Categories</h1>
                <form>
                    <div>
                        <button type="button" onClick={this.handleAdd} className="btn btn-primary">Add Category</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
