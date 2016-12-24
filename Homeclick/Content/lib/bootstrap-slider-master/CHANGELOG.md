9.5.4 / 2016-12-18
==================
* **Bug Fix:** Fixes issue where dragging slider handle outside of modal and releasing cursor would close the modal. [See original issue for further details](https://github.com/seiyria/bootstrap-slider/issues/339). Thanks to [ZeVS777](https://github.com/ZeVS777).

9.5.3 / 2016-12-01
==================
* **Bug Fix:** Fixes typo from previous update to SCSS rules. Thanks to [Julien Bachmann](https://github.com/julienbachmann).

9.5.2 / 2016-11-30
==================
* **Bug Fix:** Fixes SCSS rules. [See original issue for further details](https://github.com/seiyria/bootstrap-slider/issues/662). Thanks to [Julien Bachmann](https://github.com/julienbachmann).

9.5.1 / 2016-11-23
==================
* **Bug Fix:** Removes `'none'` classes after selection change. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/659). Thanks to [John Clarke](https://github.com/john-clarke).

9.5.0 / 2016-11-21
==================
* **New Feature:** Adds `aria-valuetext` attribute to each slider handle element, which is set to be the current formatted value of the slider (based on the `formatter` option). [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/646). Thanks to [mediaformat](https://github.com/mediaformat).

9.4.1 / 2016-11-04
==================
* **Documentation Fix:** Fixing an inconsistency with the licensing information in our source files. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/652). Thanks to [Tom Yue](https://github.com/yuethomas) for identifying this issue.

9.4.0 / 2016-10-31
==================
* **New Feature:** Adds the ability to set the slider value using stringified numbers. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/642). Thanks to [Ryan Bruns](https://github.com/snurby7)

9.3.2 / 2016-10-30
==================
* **Bug Fix:** Fixes reported bug where a slider was unable to be destroyed and re-created if there were event listeners bound to it. [See here for further details](https://github.com/seiyria/bootstrap-slider/issues/640).

9.3.0 / 2016-10-20
==================
* **New Feature:** Adds the ability to enable/disable tooltips when hovering over ticks via the `ticks_tooltip` option. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/638). Thanks to [Ryan Bruns](https://github.com/snurby7)

9.2.2 / 2016-10-18
==================
* **Bug Fix:** Resolves issue where range highlights were not being applied properly for reversed sliders. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/637). Thanks to [Bernard Gorman](https://github.com/gormanb)

9.2.0 / 2016-09-26
==================
* **New Feature:** Adding the ability to target certain ranges of the slider track via CSS in order to highlight them. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/619). Thanks to [lipoczkit](https://github.com/lipoczkit)

9.1.3 / 2016-08-06
==================
* **Bug Fix:** Checks for `window` object before attempting to attach `console` polyfills. [Resolves this issue](https://github.com/seiyria/bootstrap-slider/issues/607)

9.1.2 / 2016-08-06
==================
* Accidental publish

9.1.1 / 2016-07-15
==================
* **Bug Fix:** Adds `.npmignore` file to repository. [Resolves this issue](https://github.com/seiyria/bootstrap-slider/issues/601)

9.1.0 / 2016-07-14
==================
* **New Feature:** Always binding to the `$.fn.bootstrapSlider` namespace and printing a console warning when the `$.fn.slider` namespace is already bound. Idea came from discussion [in this issue](https://github.com/seiyria/bootstrap-slider/issues/575)

9.0.0 / 2016-07-13
==================
* **New Feature:** Wraps all of the ticks within a single container element with the class `.slider-tick-container` as opposed to being within the `.slider-track` element. This enables individual ticks to be more easily targeted with CSS selectors such as `nth-of-type(n)`. Idea came from discussion [in this issue](https://github.com/seiyria/bootstrap-slider/issues/500)

8.0.0 / 2016-07-13
==================
* **Revert:** Reverting bug fix made in `7.0.4 - 7.0.5` because it breaks UMD module definition and r.js build tool [as reported in this issue](https://github.com/seiyria/bootstrap-slider/issues/589#issuecomment-232429818). Updated README to address how to stub out optional JQuery dependency for Webpack builds.

7.1.0 - 7.1.1 / 2016-05-26
==================
* **New Feature:** Allow LESS/SASS variables to be overridden, but fall back to defaults if needed. [See here for further details](https://github.com/seiyria/bootstrap-slider/pull/579). Thanks to [Jonathan Rehm
 (jkrehm)](https://github.com/jkrehm)

7.0.4 - 7.0.5 / 2016-05-26
==================
* **Bug Fix:** Changes webpack AMD build error on define() for optional jQuery dependency to be a warning, which allows webpack builds to be completed. [See here for further details](https://github.com/seiyria/bootstrap-slider/issues/578). Thanks to [Tomi Saarinen (TomiS)](https://github.com/TomiS)

7.0.2 / 2016-04-05
==================
* **Bug Fix:** Fixes overlap issue with range slider. [See here for further details](https://github.com/seiyria/bootstrap-slider/issues/435). Thanks to [Jerry (jerrylow)](https://github.com/jerrylow)

7.0.0 / 2016-04-05
==================
* **Breaking Change:** Restructured and refactored SASS source files to eliminate compass dependency and be more organized. Thanks to [Jacob van Mourik
 (jcbvm)](https://github.com/jcbvm)

6.1.7 / 2016-04-03
==================
* **Bug Fix:** Fixes issue where slider accidently scrolls when user taps on mobile device. Thanks to [Jerry (jerrylow)](https://github.com/jerrylow)

6.1.5 / 2016-03-12
==================
* **Bug Fix:** Call resize() before layout() within relayout() method, which enables intially hidden sliders to be revealed and behave appropriately. Thanks to [Peter (MaZderMind)](https://github.com/MaZderMind)

6.1.3 / 2016-03-07
==================
* **Bug Fix:** Fixed horizontal centering issue with labels. Thanks to [Josh Guffey](https://github.com/jguffey)

6.1.0 / 2016-02-28
==================
* **New Feature:** Auto-registering/intializing slider via `data-provide="slider"` attribute. Thanks to [MaZderMind](https://github.com/MaZderMind)
* Adding Github Templates for Issues, Pull Requeusts, and Contributions

6.0.16 / 2016-02-04
==================
* **Bug Fix:** Attempted Bug fix from 6.0.11 was refined to ensure so side effects.

6.0.15 / 2016-02-04
==================
* **Bug Fix:** _setText() defaults to `.textContent` vs `.innerHTML`. Thanks to [gio-js](https://github.com/gio-js)

6.0.13 / 2016-01-31
==================
* Reverted Bug fix from prior release

6.0.11 / 2016-01-31
==================
* **Bug fix:** Slider was not scrolling properly when nested inside of scrollable container. Thanks to [serbiant](https://github.com/serbiant)


6.0.9 / 2016-01-26
==================
* **Bug fix:** Race condition in `setValue()` where slider value was being set after `change` and `slide` events were being triggered. Thanks to [glaszig](https://github.com/glaszig)

6.0.7 / 2016-01-22
==================
* **Bug fix:** When `tooltip_position` option is set to `"bottom"` on a slider with multiple split handles, position both tooltips below the slider. Thanks to [Martin Hesslund](https://github.com/kesse)

6.0.5 / 2016-01-20
==================
* bower.json: changing "main" to reference /dist/bootstrap-slider.js

6.0.2 / 2015-12-31
==================
* package.json: changing "main" to point at proper file path

6.0.0 / 2015-12-30
==================
* Moving all source code to `/src` directory
* Transpiling JS with [Babel](https://babeljs.io/)
* Adding `Other Guidelines` section to CONTRIBUTING.MD
* Updating README with Grunt CLI tasks
* Update postpublish script to reference transpiled code
* Freezing dependency versions (this allows us to ensure the module and grunt tasks always have consistent/repeatable behavior)
* Adding an `.nvmrc` file for Node 5.x.x. This version of node comes with NPM 3.x.x, which creates a flat dependency tree for `node_modules`, which basically eliminates the need for bower as our client-side deps management solution

5.3.6 / 2015-12-27
==================
* Restoring bootstrap depedency to bower.json file (Fixes issue with `grunt prod` task)

5.3.4 / 2015-12-27
==================
* **Bug fix:** Ticks now reposition themselves during window resize - Thanks to [Zachary Siswick](https://github.com/zsiswick)
