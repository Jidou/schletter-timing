import React, { Component, useState } from 'react';
import { Container } from 'reactstrap';
import { Button, Navbar, Nav, NavDropdown } from 'react-bootstrap';

import { useDarkMode } from './useDarkMode';
import { ThemeProvider } from 'styled-components';
import { lightTheme, darkTheme } from './theme';
import { GlobalStyles } from './global';


export class Layout extends Component {
  static displayName = Layout.name;

    // constructor () {
    //   const [theme, toggleTheme, componentMounted] = useDarkMode();
    //   const themeMode = theme === 'light' ? lightTheme : darkTheme;
    // }

    render () {
      return (
        <div>
          <Navbar bg="primary" variant="dark">
            <Navbar.Brand href="/">Schletter Timing</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav" className="justify-content-end">
              <Nav className="mr-auto">
                <Nav.Link href="/raceoverview">Races</Nav.Link>
                <Nav.Link href="/groups">Groups</Nav.Link>
                <Nav.Link href="/participants">Participants</Nav.Link>
                <Nav.Link href="/categories">Categories</Nav.Link>
                <Nav.Link href="/classes">Classes</Nav.Link>
                <NavDropdown title="Race" id="basic-nav-dropdown">
                  <NavDropdown.Item href="/race/race">Overview</NavDropdown.Item>
                  <NavDropdown.Item href="/race/racegroups">Groups</NavDropdown.Item>
                  {/* <NavDropdown.Item href="/race/raceparticipants">Participants</NavDropdown.Item> */}
                  <NavDropdown.Item href="/race/timing">Timing</NavDropdown.Item>
                  <NavDropdown.Item href="/race/result">Result</NavDropdown.Item>
                </NavDropdown>
              </Nav>
              {/* <ThemeProvider theme={lightTheme}>
                <Button variant="outline-info" onClick={this.toggleTheme}>Toggle theme</Button>
              </ThemeProvider> */}
            </Navbar.Collapse>
          </Navbar>


          <Container>
            {this.props.children}
          </Container>
        </div>
      );
  }
}
