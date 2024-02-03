import {
    createContext,
    ReactNode,
    useContext,
    useEffect,
    useState,
  } from "react";
  import { User } from "../models/User";
  import {
    lsGetToken,
    lsGetUser,
    lsRemoveToken,
    lsRemoveUser,
    lsSetToken,
    lsSetUser,
  } from "../utils/helpers";
  import * as jwtDecode from "jwt-decode";
  
  interface AuthState {
    user: User;
    authToken: string;
  }
  
  interface AuthStateContextI {
    user: User | null;
    signin: (authState: AuthState) => void;
    signout: () => void;
    autoLogin: () => void;
    isAuthenticated: () => boolean;
  }
  
  interface AuthStateProviderProps {
    children: ReactNode;
  }
  
  const AuthStateContext = createContext<AuthStateContextI>(
    {} as AuthStateContextI
  );
  
  export function useAuthContext() {
    const context = useContext(AuthStateContext);
    if (!context)
      throw new Error("useAuthState must be used within AuthProvider");
  
    return context;
  }
  
  export function AuthStateProvider({ children }: AuthStateProviderProps) {
    const [authState, setAuthState] = useState<AuthState | null>(null);
  
    useEffect(() => {
      autoLogin();
    }, []);
  
    const signin = (authState: AuthState) => {
      setAuthState(authState);
      lsSetToken(authState.authToken);
      lsSetUser(authState.user);
    };
  
    const signout = () => {
      if (lsGetToken() == null) {
        return;
      }
  
      setAuthState(null);
      lsRemoveToken();
      lsRemoveUser();
    };
  
    const autoLogin = () => {
      const user = lsGetUser();
      const token = lsGetToken();
  
      if (!user || !token) {
        return;
      }
  
      setAuthState({ user, authToken: token });
    };
  
    const isAuthenticated = () => {
      const token = lsGetToken();
  
      if (!token) return false;
  
      const { exp } = jwtDecode.jwtDecode<{ exp: number }>(token);
  
      if (exp) {
        if (Date.now() < exp * 1000) {
          return true;
        }
      }
  
      signout();
      return false;
    };
  
    return (
      <AuthStateContext.Provider
        value={{
          autoLogin,
          signin,
          signout,
          isAuthenticated,
          user: authState?.user ?? null,
        }}
      >
        {children}
      </AuthStateContext.Provider>
    );
  }
  