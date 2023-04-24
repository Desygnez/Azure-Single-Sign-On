import React, {useEffect, useState} from "react";
import {Headers} from "../components/Headers";
import {GradeInterface} from "../lib/Grade/Grade";
import {GradeSheet} from "../lib/GradeSheet/GradSheet";
import {GetGradesByUserId} from "../lib/GradeSheet/GradeSheetService";
import CalculateAverage from "../lib/services/AverageCalculator";
import {GetCurrentUser, GetUsers} from "../lib/UserInfo/UserInformationService";
import {User, UserInfoInterface} from "../lib/UserInfo/UserInfo";
import UserTable from "../components/UserTable";

/**
 * Renders the Home Page which, when selected, will redirect the page
 */

const defaultUser: User = {
    apprentices: [], email: "", firstname: "", id: "", isVocationalTrainer: false, lastname: "", username: ""

}

const defaultApprentice: UserInfoInterface = {
    email: "",
    firstLogin: false,
    firstname: "",
    id: "",
    lastname: "",
    password: "",
    role_id: "",
    roles: {id: "1", role: "test"},
    username: ""
}
export default function Home(): JSX.Element {
    document.title = "Home - KPMG Notentool";
    const [isAverageEligable, setIsAverageEligable] = useState<boolean>(true);
    const [gradeSheet, setGradeSheet] = useState<GradeSheet>();
    const [average, setAverage] = useState<number | string>();
    const [user, setUser] = useState<User>(defaultUser);
    const [apprentices, setApprentices] = useState<UserInfoInterface[]>([defaultApprentice]);

    useEffect(() => {
        GetCurrentUser().then(value => setUser(value));

        const loadGradeSheet = async () => {
            const loadedUser = await GetCurrentUser();
            setUser(loadedUser)
            if (!loadedUser?.isVocationalTrainer && loadedUser?.apprentices.length == 0) {
                const gradeSheet: GradeSheet = await GetGradesByUserId(
                    loadedUser.id
                );

                setGradeSheet(gradeSheet);
                let gradeArray: GradeInterface[] = [];
                gradeSheet.forEach(function (item) {
                    gradeArray.push(item.grades);
                });
                setAverage(CalculateAverage(...gradeArray));

                setIsAverageEligable(true);
            } else {
                setIsAverageEligable(false);
                const users = await GetUsers();
                setApprentices(users)
            }
        };

        loadGradeSheet();
    }, []);

    return (
        <div>
            <Headers title={`Hello ${user.firstname}, Welcome to Notentool`}/>
            {isAverageEligable ? (
                <div className="dark:text-white grid grid-cols-5">
                    <div className="col-start-2 col-end-5 grid grid-rows-2 gap-5 mt-5">
                        <h1 className="text-3xl">Your total average:</h1>
                        <h1 className="text-3xl font-bold">{average}</h1>
                    </div>
                </div>
            ) : (
                <div className={"mt-5"}>
                    <UserTable users={apprentices}/>
                </div>
            )}
        </div>
    );
}