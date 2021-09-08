import * as actionTypes from "../types/actionTypes";

export default (tests = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_ALL:
            return tests;
        case actionTypes.CREATE:
            return tests;
        default:
            return tests;
    }
}