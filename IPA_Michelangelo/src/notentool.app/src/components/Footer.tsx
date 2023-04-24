import React from 'react';

function Footer() {
    return (
        <div>
            <div className={"h-2 bg-[#1E49E2]"}></div>
            <footer className="footer flex items-center p-3 bg-slate-300 text-slate-900 text-[10pt]">
                <span>
                <p>&copy; {new Date().getFullYear()} KPMG AG - <a href={"https://kpmgche.sharepoint.com/sites/CH-OI-INF-junior-power"} className={"text-[#00b8f5] font-bold cursor-pointer"}>Powered & developed by DBT Young Talents</a></p>
                </span>
                <img className={"ml-5 h-[55px]"}
                     src={"/DBTYTLogo.png"}
                     alt={"Young Talents logo"}/>
            </footer>
        </div>

    );
}

export default Footer;