import { Home } from "./pages/Home";
import Test from "./pages/Test";
import Error from "./pages/error";

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
    path: '/*',
    element: <Error />
  }
];

export default AppRoutes;
