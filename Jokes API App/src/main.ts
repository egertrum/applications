import Aurelia from 'aurelia';
import { RouterConfiguration } from "aurelia-direct-router";
import { MyApp } from './my-app';

import 'jquery';
import 'popper.js';
import 'bootstrap';

import 'bootstrap/dist/css/bootstrap.min.css'
import 'font-awesome/css/font-awesome.min.css'
import '../static/site.css'


Aurelia
  .register(RouterConfiguration.customize({ useUrlFragmentHash: false }))
  .app(MyApp)
  .start();
