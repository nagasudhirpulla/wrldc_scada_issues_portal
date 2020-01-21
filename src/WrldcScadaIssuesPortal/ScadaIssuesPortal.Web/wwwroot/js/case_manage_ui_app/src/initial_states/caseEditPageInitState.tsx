import { ICaseEditPageState } from "../type_defs/ICaseEditPageState";

const initState: ICaseEditPageState = {
    info: {
        caseItems: [],
        concernedAgencies: [],
        comments: [],
        downTime: "",
        resolutionTime: "",
        createdAt: "",
        updatedAt: "",
        createdById: "6fa0893c-e8bc-43cd-99a2-3890ac0a55fd",
        attachmentName: null,
        attachmentPath: null,
        id: 25
    },
    baseAddr: "../..",
    users: [],
    commentTagTypes: []
};

export default initState;
