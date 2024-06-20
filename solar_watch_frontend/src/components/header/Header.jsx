import "./Header.css";
import { useRef, useState, useContext, useEffect } from "react";
import { AuthContext } from "../root/Root.jsx";
import { Link, useNavigate } from "react-router-dom";

export default function Header() {
  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [isSearchActive, setIsSearchActive] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [fetchedCities, setFetchedCities] = useState([]);
  const [headerEmail, setHeaderEmail] = useState(null);
  const [headerPassword, setHeaderPassword] = useState(null);
  const [headerIsLoginSuccessful, setHeaderIsLoginSuccessful] = useState(null);
  const [showMobileMenu, setShowMobileMenu] = useState(false);
  const dropdownRef = useRef(null);
  const menuSearchDiv = useRef(null);
  const menuSearchInput = useRef(null);
  const { user, login, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  function toggleLogin() {
    setIsLoginOpen((prevState) => !prevState);
  }

  async function toggleSearch() {
    if (isSearchActive && searchTerm != "") {
      console.log("implement searching logic here");
      navigate(`/search/${searchTerm}`);
      //await fetchCityName();
      setSearchTerm("");
      setFetchedCities([]);
    }
    setIsSearchActive((prevState) => !prevState);
  }

  const fetchCityName = async () => {
    const url = "https://test.api.amadeus.com/v1/security/oauth2/token";
    const apiKey = "p9F9WD4AX2q1aWZA4zA0PBIfiq1FJjFy";
    const apiSecret = "LvGxe9T2bnK4UCxT";
    const data = new URLSearchParams();
    data.append("grant_type", "client_credentials");
    data.append("client_id", apiKey);
    data.append("client_secret", apiSecret);

    let responseContent;
    try {
      const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: data,
      });
      if (!res.ok) {
        throw new Error("Authorization response was not ok.");
      }
      responseContent = await res.json();
    } catch (error) {
      console.log(error);
    }

    // fetch the cities
    const cityUrl = `https://test.api.amadeus.com/v1/reference-data/locations/cities?keyword=${searchTerm}&max=10`;
    try {
      const cityResponse = await fetch(cityUrl, {
        method: "GET",
        headers: { Authorization: `Bearer ${responseContent.access_token}` },
      });
      if (!cityResponse.ok) {
        throw new Error("Data fetching response was not ok.");
      }
      const cityResponseContent = await cityResponse.json();
      console.log(cityResponseContent.data);
      setFetchedCities(cityResponseContent.data);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {}, [fetchedCities]);

  useEffect(() => {
    if (isSearchActive) {
      menuSearchInput.current.focus();
    }
  }, [isSearchActive]);

  function handleLogout() {
    logout();
    setIsLoginOpen((prevState) => !prevState);
  }

  async function onHeaderLogin(e) {
    e.preventDefault();
    const userToLogin = {
      email: headerEmail,
      password: headerPassword,
    };
    if (await login(userToLogin)) {
      setIsLoginOpen((prevState) => !prevState);
      setHeaderIsLoginSuccessful(true);
    } else {
      setHeaderIsLoginSuccessful(false);
    }
  }

  useEffect(() => {
    if (searchTerm.length > 2) {
      fetchCityName();
    }
  }, [searchTerm]);

  const searchLinkClick = () => {
    setFetchedCities([]);
    setIsSearchActive((prev) => !prev);
    setSearchTerm("");
  };

  return (
    <header>
      <div id="navigation">
        <button id="mobile-menu-btn" onClick={() => {
          setShowMobileMenu(prev => !prev);
          if (isSearchActive){
            setIsSearchActive(false);
          }
        }}>
        </button>
        <Link className="logo" to="/" >Logo</Link>
        {showMobileMenu && (
          <div id="mobile-menu-wrapper">
            <div id="mobile-menu-header">
              <div className="mobile-menu-header-item">
                <h3>Kategóriák</h3>
              </div>
              <div className="mobile-menu-header-item" onClick={() => {
                setShowMobileMenu(prev => !prev);
              } }>
                <h3>Vissza</h3>
              </div>
            </div>
            <ul id="mobile-menu-list">
              <li>
                <Link to="/" onClick={() => setShowMobileMenu(false)}>Home</Link>
              </li>
              <li>
                <Link to="/cities" onClick={() => setShowMobileMenu(false)}>Cities</Link>
              </li>
            </ul>
          </div>
        )}
        <ul className="menu">
          <li>
            <Link to="/">Home</Link>
          </li>
          <li>
            <Link to="/cities">Cities</Link>
          </li>
        </ul>
        <div className="user-menu">
          <div id="search">
            <div onClick={toggleSearch} className="user-menu-items">
              <svg
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeWidth="2"
                  d="m21 21-3.5-3.5M17 10a7 7 0 1 1-14 0 7 7 0 0 1 14 0Z"
                />
              </svg>
              <span className="user-menu-span">Search</span>
            </div>
            {isSearchActive && (
              <>
                <div className={"user-menu-search"} ref={menuSearchDiv}>
                  <input
                    type="text"
                    ref={menuSearchInput}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    value={searchTerm}
                  />
                </div>
                {fetchedCities && fetchedCities.length > 0 && (
                  <div id="quick-search">
                    <div id="information">
                      <span>
                        A teljes keresési eredmény megtekintéséhez nyomja meg az
                        ENTER vagy KERESÉS gombot!
                      </span>
                    </div>
                    <div id="quick-search-suggestions">
                      <span>Keresési javaslatok</span>
                      {fetchedCities.map((city) => (
                        <Link
                          key={city.iataCode}
                          to={`/cities/${city.name}`}
                          onClick={searchLinkClick}
                        >
                          {city.name}
                        </Link>
                      ))}
                    </div>
                  </div>
                )}
              </>
            )}
          </div>
          <div id="account">
            <div onClick={toggleLogin} className="user-menu-items">
              <svg
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M12 21a9 9 0 1 0 0-18 9 9 0 0 0 0 18Zm0 0a8.949 8.949 0 0 0 4.951-1.488A3.987 3.987 0 0 0 13 16h-2a3.987 3.987 0 0 0-3.951 3.512A8.948 8.948 0 0 0 12 21Zm3-11a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                />
              </svg>
              <span className="user-menu-span">
                {user != null ? user.userName : "Account"}
              </span>
            </div>

            {isLoginOpen && (
              <div className="dropdown-menu" ref={dropdownRef}>
                {user == null ? (
                  <>
                    <h4>Login</h4>
                    <form onSubmit={onHeaderLogin}>
                      <label>
                        <span>Email</span>
                        <input
                          type="text"
                          name="email"
                          placeholder="Email"
                          onChange={(e) => setHeaderEmail(e.target.value)}
                        />
                      </label>
                      <label>
                        <span>Password</span>
                        <input
                          type="password"
                          name="password"
                          placeholder="Password"
                          onChange={(e) => setHeaderPassword(e.target.value)}
                        />
                      </label>

                      <button id="submit-btn" type="submit">
                        Login
                      </button>
                    </form>
                    {headerIsLoginSuccessful === false && (
                      <div>
                        <p>Login is not successful, try again!</p>
                      </div>
                    )}
                  </>
                ) : (
                  <>
                    <h4>Welcome, {user.userName}</h4>
                    <ul>
                      <li>Account</li>
                      <li>Change password</li>
                      <li onClick={handleLogout}>Logout</li>
                    </ul>
                  </>
                )}
              </div>
            )}
          </div>
        </div>
      </div>
    </header>
  );
}
