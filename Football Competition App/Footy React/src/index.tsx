import './admin2.css';

import 'jquery';
import 'popper.js';
import 'bootstrap';

import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { BrowserRouter as Router } from 'react-router-dom';

// import reportWebVitals from './reportWebVitals';

ReactDOM.render(
    <Router basename={process.env.PUBLIC_URL}>
        <React.StrictMode>
            <App />
        </React.StrictMode>
    </Router>,
    document.getElementById('root')
);


