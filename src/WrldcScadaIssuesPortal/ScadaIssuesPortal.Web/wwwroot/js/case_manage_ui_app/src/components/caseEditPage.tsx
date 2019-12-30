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
                    const labelComp = (<>
                        <label key={`caseQues_${caseItemInd}`} className="question">{caseItem.question}</label>
                        <input ref={register} defaultValue={caseItem.id} name={`caseItemId[${caseItemInd}]`} key={`caseId_${caseItemInd}`} style={{ display: "none" }}></input>
                    </>);
                    let inpComp = <></>;
                    const formItemName = `caseItemResp[${caseItemInd}]`;
                    const defValue = caseItem.response;
                    if ([0, 3, 4].includes(caseItem.responseType)) { // ShortText, Choices, ChoicesWithText
                        inpComp = <input key={`caseResp_${caseItemInd}`} defaultValue={defValue} className="form-control" type="text" name={formItemName} ref={register({ maxLength: 250 })} />
                    } else if (caseItem.responseType == 1) { // LongText
                        inpComp = <textarea key={`caseResp_${caseItemInd}`} defaultValue={defValue} className="form-control" name={formItemName} ref={register({ maxLength: 700 })} />
                    } else if (caseItem.responseType == 2) { // DateTime
                        inpComp = <input key={`caseResp_${caseItemInd}`} defaultValue={defValue} className="form-control datetimepicker" name={formItemName} ref={register({ required: true })} />
                    }
                    return (
                        <>
                            {labelComp}
                            {inpComp}
                        </>
                    );
                })}
                <label className="question">Issue Time</label>
                <input className="form-control datetimepicker" defaultValue={pageState.info.downTime} name="issue_time" ref={register({ required: true })} />

                <label className="question">Concerned Agencies</label>
                {/*https://medium.com/@lahiru0561/react-select-with-custom-label-and-custom-search-122bfe06b6d7*/}

                {pageState.users.length > 0 && <Select
                    defaultValue={
                        pageState.users.filter(
                            us => { return pageState.info.concernedAgencies.some(ca => ca.userId == us.id) }
                        ).map(
                            usr => { return { label: usr.userName, value: usr.id } }
                        )
                    }
                    isMulti
                    name="concernedAgencies"
                    options={pageState.users.map(us => { return { label: us.userName, value: us.id } })}
                    className="basic-multi-select"
                    classNamePrefix="select"
                />}

                <label className="question">Comments</label>
                {pageState.info.comments.length == 0 && <span>No comments recieved yet...</span>}
                {
                    pageState.info.comments.map((comm, commInd) => {
                    return (<p>{`${comm.created} ${pageState.users.find(usr => usr.id == comm.createdById).userName} ${comm.tag.toString()} ${comm.comment}`}</p>)
                }
                )}

                <input type="submit" />
            </form>
        </>
    );
}

export default CaseEditPage;