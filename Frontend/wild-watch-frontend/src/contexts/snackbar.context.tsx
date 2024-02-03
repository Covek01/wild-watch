import { Alert, Snackbar } from "@mui/material";
import { useState, ReactNode, createContext, useContext } from "react";

interface SnackBarProviderProps {
  children: ReactNode;
}

interface ISnackbarContext {
  openSnackbar: (props: OpenSnackbarProps) => void;
  closeSnackbar: () => void;
}

interface OpenSnackbarProps {
  message: string;
  autoHideDuration?: number;
  severity?: "success" | "warning" | "info" | "error";
}

const SnackbarContext = createContext<ISnackbarContext>({} as ISnackbarContext);

export function useSnackbar() {
  const context = useContext(SnackbarContext);
  if (!context)
    throw new Error("useSnackbar must be used within SnackbarProvider");

  return context;
}

export function SnackbarProvider({ children }: SnackBarProviderProps) {
  const [state, setState] = useState<OpenSnackbarProps & { open: boolean }>({
    open: false,
    message: "",
    autoHideDuration: 3000,
    severity: "info",
  });

  const openSnackbar = ({
    message = "",
    severity = "info",
    autoHideDuration = 5000,
  }: OpenSnackbarProps) => {
    setState({ open: true, message, severity, autoHideDuration });
  };

  const closeSnackbar = () => {
    setState((prev) => ({
      ...prev,
      open: false,
    }));
  };

  return (
    <SnackbarContext.Provider value={{ openSnackbar, closeSnackbar }}>
      <Snackbar
        open={state.open}
        autoHideDuration={state?.autoHideDuration}
        onClose={closeSnackbar}
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
      >
        <Alert severity={state?.severity} onClose={closeSnackbar}>
          {state.message}
        </Alert>
      </Snackbar>
      {children}
    </SnackbarContext.Provider>
  );
}
