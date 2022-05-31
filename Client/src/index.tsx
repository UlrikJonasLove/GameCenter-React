import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import { App } from './App';
import reportWebVitals from './reportWebVitals';
import '../node_modules/bootstrap/dist/css/bootstrap.css'
import ErrorBoundary from './components/errorHandling/ErrorBoundry';
import { ErrorUI } from './components/errorHandling/ui/ErrorUI';

ReactDOM.render(
  <React.StrictMode>
    <ErrorBoundary errorUI={<ErrorUI />}>
      <App />
    </ErrorBoundary>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();