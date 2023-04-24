import {useEffect} from "react";
import {GetCurrentUser} from "../UserInfo/UserInformationService";
import {User} from "../UserInfo/UserInfo";
import {useNavigate} from "react-router-dom";

export async function useRedirectToDashboard() {
    let user: User | null = null
    const navigate = useNavigate()

    useEffect(() => {
            const load = async () => {
                user = await GetCurrentUser();
                if (user != null && !user.isVocationalTrainer && user.apprentices.length == 0) navigate(`/dashboard/${user.id}`)
            }
            load()
    }, []);

}