import Link from "next/link";

import { Button } from "@/components/ui/button";
import { routes } from "@/lib/routes";

import { ModeToggle } from "./mode-toggle";

export function Header() {
  return (
    <header className="bg-background/60 sticky top-0 z-50 py-4 backdrop-blur-sm">
      <div className="container mx-auto flex justify-between gap-8">
        <Link href={routes.home} className="font-medium">
          ChatAnalyzer
        </Link>

        <div className="flex items-center gap-4">
          <ModeToggle />

          <nav>
            <ul className="flex items-center gap-4">
              <li>
                <Button asChild variant="secondary">
                  <Link href={routes.register}>Sign Up</Link>
                </Button>
              </li>

              <li>
                <Button asChild>
                  <Link href={routes.login}>Log In</Link>
                </Button>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </header>
  );
}
