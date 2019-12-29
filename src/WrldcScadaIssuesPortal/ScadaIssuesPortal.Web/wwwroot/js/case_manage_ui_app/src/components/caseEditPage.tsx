import pageInitState from '../initial_states/caseEditPageInitState';
import { useCaseEditPageReducer } from '../reducers/caseEditPageReducer';
import * as actionTypes from '../actions/actionTypes';
import React from 'react';
import { useForm } from "react-hook-form";
import { ICaseEditPageState } from '../type_defs/ICaseEditPageState';
import Select from 'react-select';

// getting it from variable initialized from view bag
declare var _caseId: number;

function CaseEditPage() {
    pageInitState.info.id = _caseId;
    let [pageState, pageStateDispatch] = useCaseEditPageReducer(pageInitState);
    const { register, handleSubmit, errors } = useForm<ICaseEditPageState>();
    const onSubmit = data => console.log(data);
    console.log(errors);
    return (
        <>
            {/*
            <div>
                <p>Id is {pageState.info.id}</p>
                <button onClick={() => pageStateDispatch({ type: actionTypes.incrementAction })}>+1</button>
                <button onClick={() => pageStateDispatch({ type: actionTypes.decrementAction })}>-1</button>
            </div>
            <div>
                <pre>{JSON.stringify(pageState, null, 4)}</pre>
            </div>
            */}
            <form onSubmit={handleSubmit(onSubmit)}>
                {pageState.info.caseItems.map((caseItem, caseItemInd) => {
                    const labelComp = <label className="question">{caseItem.question}</label>;
                    let inpComp = <></>;
                    const formItemName = `caseItem_${caseItem.id.toString()}`;
                    const defValue = caseItem.response;
                    if ([0, 3, 4].includes(caseItem.responseType)) { // ShortText, Choices, ChoicesWithText
                        inpComp = <input key={`caseItem_${caseItemInd}`} defaultValue={defValue} className="form-control" type="text" name={formItemName} ref={register({ maxLength: 250 })} />
                    } else if (caseItem.responseType == 1) { // LongText
                        inpComp = <textarea key={`caseItem_${caseItemInd}`} defaultValue={defValue} className="form-control" name={formItemName} ref={register({ maxLength: 700 })} />
                    } else if (caseItem.responseType == 2) { // DateTime
                        inpComp = <input key={`caseItem_${caseItemInd}`} defaultValue={defValue} className="form-control datetimepicker" name={formItemName} ref={register({ required: true })} />
                    }
                    return (
                        <>
                            {labelComp}
                            {inpComp}
                        </>
                    );
                })}
                <label className="question">Issue Time</label>
                <input className="form-control datetimepicker" /*defaultValue={pageState.info.downTime.toUTCString()}*/ name="issue_time" ref={register({ required: true })} />

                <label className="question">Concerned Agencies</label>
                <Select
                    getOptionLabel={option => `${option.userName}`}
                    defaultValue={pageState.info.concernedAgencies.map((ca) => pageState.users.find((us) => us.id == ca.userId))}
                    isMulti
                    name="colors"
                    options={pageState.users}
                    className="basic-multi-select"
                    classNamePrefix="select"
                />
                <input type="submit" />
            </form>
        </>
    );
}

export default CaseEditPage;