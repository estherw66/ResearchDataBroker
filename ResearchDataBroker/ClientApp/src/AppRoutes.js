import { Home } from "./pages/Home";
import Test from "./pages/Test";
import Error from "./pages/error";
import SearchPage from "./pages/Search";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/test-api',
    element: <Test />
  },
  {
    path: '/search',
    element: <SearchPage />
  },
  {
    path: '/*',
    element: <Error />
  }
];

export default AppRoutes;
